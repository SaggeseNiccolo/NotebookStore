﻿using System.ComponentModel.DataAnnotations;

namespace NotebookStore.Business;

public class CpuDto
{
	public int Id { get; set; }
	[MaxLength(50)]
	public string? Brand { get; set; }
	[MaxLength(50)]
	public string? Model { get; set; }
}
