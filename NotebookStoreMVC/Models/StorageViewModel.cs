﻿using System.ComponentModel.DataAnnotations;

namespace NotebookStoreMVC.Models;

public class StorageViewModel
{
  public int Id { get; set; }

  /// <summary>
  /// Type of the storage (e.g. SSD, HDD)
  /// </summary>
  [MaxLength(10)]
  public required string Type { get; set; }

  /// <summary>
  /// Capacity of the storage in gigabytes (e.g. 256, 512)
  /// </summary>
  [Range(128, 4096)]
  public required int Capacity { get; set; }
  public bool CanUpdateAndDelete { get; set; }

  public string TypeAndCapacity => $"{Type} {Capacity}GB";
}
