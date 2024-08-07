﻿namespace NotebookStore.Business;

public class BrandDto : IAuditableDto
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public bool CanUpdate { get; set; }
	public bool CanDelete { get; set; }
}
