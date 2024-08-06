﻿using System.Security.Cryptography;
using IbmImporter.Models;
using NotebookStore.DAL;

namespace IbmImporter;

public class DataImporter
{
	private readonly IJsonFileParser parser;
	private readonly IValidator<Notebook> validator;
	private readonly IRepository<NotebookStore.Entities.Notebook> notebookRepository;

	public DataImporter(
		IJsonFileParser parser,
		IValidator<Notebook> validator,
		IRepository<NotebookStore.Entities.Notebook> notebookRepository)
	{
		this.parser = parser;
		this.validator = validator;
		this.notebookRepository = notebookRepository;
	}

	public ImportResult Import(string file)
	{
		// Parsing file json
		// Validare
		//      Se non è valido aggiungere UnimportedNotebook all'ImportResult
		//      Se valido andare avanti
		// Importare Notebook validi
		//      Se l'importazione non va a buon fine aggiugnere UnimportedNotebook ad ImportResult
		//      Se l'importazione va a buon fine andare avanti
		// Ritornare ImportResult

		var importResult = new ImportResult();

		var notebookData = parser.Parse(file);

		notebookData?.Notebooks.ForEach(async notebook =>
		{
			var validationResult = validator.Validate(notebook);

			if (string.IsNullOrEmpty(validationResult))
			{
				var importNotebookResult = await ImportNotebook(notebook, notebookData.Customer);

				if (!string.IsNullOrEmpty(importNotebookResult))
				{
					importResult.Unsuccess.Add(new UnimportedNotebook
					{
						Index = notebookData.Notebooks.IndexOf(notebook),
						ErrorMessage = importNotebookResult,
						Notebook = notebook
					});
				}
				else
				{
					importResult.Success++;
				}
			}
			else
			{
				importResult.Unsuccess.Add(new UnimportedNotebook
				{
					Index = notebookData.Notebooks.IndexOf(notebook),
					ErrorMessage = validationResult,
					Notebook = notebook
				});
			}
		});

		return importResult;
	}

	private async Task<string> ImportNotebook(Notebook notebook, string customer)
	{
		var culture = new System.Globalization.CultureInfo("it-IT");

		NotebookStore.Entities.Notebook notebookEntity = new()
		{
			Color = notebook.Color,
			Price = (int)notebook.Price,
			Brand = new()
			{
				Name = notebook.Name,
				CreatedBy = customer,
				CreatedAt = DateTime.Now.ToString(culture)
			},
			Cpu = new()
			{
				Model = notebook.ProcessorModel,
				CreatedBy = customer,
				CreatedAt = DateTime.Now.ToString(culture)
			},
			Display = new()
			{
				ResolutionWidth = notebook.Monitor.Width,
				ResolutionHeight = notebook.Monitor.Height,
				CreatedBy = customer,
				CreatedAt = DateTime.Now.ToString(culture)
			},
			Memory = new()
			{
				Capacity = notebook.Ram,
				CreatedBy = customer,
				CreatedAt = DateTime.Now.ToString(culture)
			},
			Model = new()
			{
				CreatedAt = DateTime.Now.ToString(culture),
			},
			Storage = new()
			{
				// Capacità casuale per evitare unique constraint del database 
				// (perché non viene passata la capacità dello storage nel json)
				Capacity = RandomNumberGenerator.GetInt32(128, 1024),
				CreatedBy = customer,
				CreatedAt = DateTime.Now.ToString(culture)
			},
			CreatedBy = customer,
			CreatedAt = DateTime.Now.ToString(culture)
		};

		try
		{
			await notebookRepository.Create(notebookEntity);
		}
		catch (Exception e)
		{
			if (e.InnerException != null)
			{
				return e.InnerException.Message;
			}

			return e.Message;
		}

		return string.Empty;
	}
}
