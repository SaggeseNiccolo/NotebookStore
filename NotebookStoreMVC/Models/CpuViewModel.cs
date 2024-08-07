﻿using System.ComponentModel.DataAnnotations;

namespace NotebookStoreMVC.Models;

public class CpuViewModel
{
  public int Id { get; set; }
  [MaxLength(50)]
  public string? Brand { get; set; }
  [MaxLength(50)]
  public string? Model { get; set; }
  public bool CanUpdateAndDelete { get; set; }

  public string Name => $"{Brand} {Model}";
}
