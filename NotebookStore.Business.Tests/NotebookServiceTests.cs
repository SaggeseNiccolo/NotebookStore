﻿using Moq;
using NotebookStore.Entities;

namespace NotebookStore.Business.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class NotebookServiceTests
{
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        TestStartup.Register<NotebookService>();
    }

    [Test]
    public async Task GetAll_ReturnsNotebooks()
    {
        using var context = TestStartup.CreateComponentsContext();

        var sut = context.Resolve<NotebookService>();

        // Arrange
        var notebook1 = new Notebook
        {
            Id = 1,
            Brand = new Brand
            {
                Id = 1,
                Name = "Brand 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            BrandId = 1,
            Model = new Model
            {
                Id = 1,
                Name = "Model 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            ModelId = 1,
            Cpu = new Cpu
            {
                Id = 1,
                Brand = "Intel",
                Model = "Core i5",
                CreatedAt = DateTime.Now.ToString(),
            },
            CpuId = 1,
            Display = new Display
            {
                Id = 1,
                Size = 15.6,
                ResolutionWidth = 1920,
                ResolutionHeight = 1080,
                PanelType = "IPS",
                CreatedAt = DateTime.Now.ToString(),
            },
            DisplayId = 1,
            Memory = new Memory
            {
                Id = 1,
                Capacity = 8,
                Speed = 3200,
                CreatedAt = DateTime.Now.ToString(),
            },
            MemoryId = 1,
            Storage = new Storage
            {
                Id = 1,
                Capacity = 512,
                Type = "SSD",
                CreatedAt = DateTime.Now.ToString(),
            },
            StorageId = 1,
            Color = "Black",
            CreatedAt = DateTime.Now.ToString(),
            CreatedBy = null
        };

        var notebook2 = new Notebook
        {
            Id = 2,
            Brand = new Brand
            {
                Id = 2,
                Name = "Brand 2",
                CreatedAt = DateTime.Now.ToString(),
            },
            BrandId = 2,
            Model = new Model
            {
                Id = 2,
                Name = "Model 2",
                CreatedAt = DateTime.Now.ToString(),
            },
            ModelId = 2,
            Cpu = new Cpu
            {
                Id = 2,
                Brand = "AMD",
                Model = "Ryzen 5",
                CreatedAt = DateTime.Now.ToString(),
            },
            CpuId = 2,
            Display = new Display
            {
                Id = 2,
                Size = 14,
                ResolutionWidth = 1920,
                ResolutionHeight = 1080,
                PanelType = "IPS",
                CreatedAt = DateTime.Now.ToString(),
            },
            DisplayId = 2,
            Memory = new Memory
            {
                Id = 2,
                Capacity = 16,
                Speed = 3200,
                CreatedAt = DateTime.Now.ToString(),
            },
            MemoryId = 2,
            Storage = new Storage
            {
                Id = 2,
                Capacity = 1024,
                Type = "SSD",
                CreatedAt = DateTime.Now.ToString(),
            },
            StorageId = 2,
            Color = "White",
            CreatedAt = DateTime.Now.ToString(),
            CreatedBy = null
        };

        Mock.Get(context.UserService)
            .Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new UserDto
            {
                Id = "1",
                Name = "User 1",
                Email = "",
                Password = "",
                Role = "Admin"
            });

        Mock.Get(context.PermissionService)
            .Setup(x => x.CanUpdateNotebook(
                It.IsAny<Notebook>(),
                It.IsAny<UserDto>()
            ))
            .Returns(true);

        context.DbContext.AddRange(notebook1, notebook2);
        context.DbContext.SaveChanges();

        // Act
        var result = await sut.GetAll();
        var notebook1Result = result.FirstOrDefault(n => n.Id == notebook1.Id);
        var notebook2Result = result.FirstOrDefault(n => n.Id == notebook2.Id);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2), "Notebooks count should be 2");

        Assert.Multiple(() =>
        {
            Assert.That(notebook1Result?.Id, Is.EqualTo(1), "Notebook 1 Id should be 1");
            Assert.That(notebook1Result?.BrandId, Is.EqualTo(1), "Notebook 1 BrandId should be 1");
            Assert.That(notebook1Result?.Color, Is.EqualTo("Black"), "Notebook 1 Color should be Black");
            Assert.That(notebook1Result?.CanUpdate, Is.True, "Notebook 1 CanUpdate should be true");
            Assert.That(notebook1Result?.CanDelete, Is.True, "Notebook 1 CanDelete should be true");
        });

        Assert.Multiple(() =>
        {
            Assert.That(notebook2Result?.Id, Is.EqualTo(2), "Notebook 2 Id should be 2");
            Assert.That(notebook2Result?.BrandId, Is.EqualTo(2), "Notebook 2 BrandId should be 2");
            Assert.That(notebook2Result?.Color, Is.EqualTo("White"), "Notebook 2 Color should be White");
            Assert.That(notebook2Result?.CanUpdate, Is.True, "Notebook 2 CanUpdate should be true");
            Assert.That(notebook2Result?.CanDelete, Is.True, "Notebook 2 CanDelete should be true");
        });
    }

    [Test]
    public async Task Find_ReturnsNotebook()
    {
        using var context = TestStartup.CreateComponentsContext();

        var sut = context.Resolve<NotebookService>();

        // Arrange
        var notebook = new Notebook
        {
            Id = 1,
            BrandId = 1,
            Brand = new Brand
            {
                Id = 1,
                Name = "Brand 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            ModelId = 1,
            Model = new Model
            {
                Id = 1,
                Name = "Model 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            CpuId = 1,
            Cpu = new Cpu
            {
                Id = 1,
                Brand = "Intel",
                Model = "Core i5",
                CreatedAt = DateTime.Now.ToString(),
            },
            DisplayId = 1,
            Display = new Display
            {
                Id = 1,
                Size = 15.6,
                ResolutionWidth = 1920,
                ResolutionHeight = 1080,
                PanelType = "IPS",
                CreatedAt = DateTime.Now.ToString(),
            },
            MemoryId = 1,
            Memory = new Memory
            {
                Id = 1,
                Capacity = 8,
                Speed = 3200,
                CreatedAt = DateTime.Now.ToString(),
            },
            StorageId = 1,
            Storage = new Storage
            {
                Id = 1,
                Capacity = 512,
                Type = "SSD",
                CreatedAt = DateTime.Now.ToString(),
            },
            Color = "Black",
            CreatedAt = DateTime.Now.ToString(),
            CreatedBy = null
        };

        Mock.Get(context.UserService)
            .Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new UserDto
            {
                Id = "1",
                Name = "User 1",
                Email = "",
                Password = "",
                Role = "Admin"
            });

        Mock.Get(context.PermissionService)
            .Setup(x => x.CanUpdateNotebook(
                It.IsAny<Notebook>(),
                It.IsAny<UserDto>()
            ))
            .Returns(true);

        context.DbContext.Add(notebook);
        context.DbContext.SaveChanges();

        // Act
        var result = await sut.Find(notebook.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result?.Id, Is.EqualTo(notebook.Id), "Notebook Id should be 1");
            Assert.That(result?.BrandId, Is.EqualTo(notebook.BrandId), "Notebook BrandId should be 1");
            Assert.That(result?.Color, Is.EqualTo(notebook.Color), "Notebook Color should be Black");
            Assert.That(result?.CanUpdate, Is.True, "Notebook CanUpdate should be true");
            Assert.That(result?.CanDelete, Is.True, "Notebook CanDelete should be true");
        });
    }

    [Test]
    public async Task Create_ReturnsNotebook()
    {
        using var context = TestStartup.CreateComponentsContext();

        var sut = context.Resolve<NotebookService>();

        // Arrange
        var notebook = new NotebookDto
        {
            Id = 1,
            BrandId = 1,
            ModelId = 1,
            CpuId = 1,
            DisplayId = 1,
            MemoryId = 1,
            StorageId = 1,
            Color = "Black",
            Price = 1000
        };

        Mock.Get(context.UserService)
            .Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new UserDto
            {
                Id = "1",
                Name = "User 1",
                Email = "",
                Password = "",
                Role = "Admin"
            });

        Mock.Get(context.PermissionService)
            .Setup(x => x.CanUpdateNotebook(
                It.IsAny<Notebook>(),
                It.IsAny<UserDto>()
            ))
            .Returns(true);

        context.DbContext.Brands.Add(new Brand
        {
            Id = 1,
            Name = "Brand 1",
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.Models.Add(new Model
        {
            Id = 1,
            Name = "Model 1",
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.Cpus.Add(new Cpu
        {
            Id = 1,
            Brand = "Intel",
            Model = "Core i5",
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.Displays.Add(new Display
        {
            Id = 1,
            Size = 15.6,
            ResolutionWidth = 1920,
            ResolutionHeight = 1080,
            PanelType = "IPS",
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.Memories.Add(new Memory
        {
            Id = 1,
            Capacity = 8,
            Speed = 3200,
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.Storages.Add(new Storage
        {
            Id = 1,
            Capacity = 512,
            Type = "SSD",
            CreatedAt = DateTime.Now.ToString(),
        });

        context.DbContext.SaveChanges();

        // Act
        var result = await sut.Create(notebook);
        var addedNotebook = context.DbContext.Notebooks.FirstOrDefault(n => n.Id == notebook.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, "Create failed");
            Assert.That(addedNotebook?.Id, Is.EqualTo(notebook.Id), "Notebook id should be the same");
            Assert.That(addedNotebook?.BrandId, Is.EqualTo(notebook.BrandId), "Notebook brand id should be the same");
        });
    }

    [Test]
    public async Task Update_ReturnsNotebook()
    {
        using var context = TestStartup.CreateComponentsContext();

        var sut = context.Resolve<NotebookService>();

        // Arrange
        var notebook = new Notebook
        {
            Id = 1,
            BrandId = 1,
            Brand = new Brand
            {
                Id = 1,
                Name = "Brand 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            ModelId = 1,
            Model = new Model
            {
                Id = 1,
                Name = "Model 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            CpuId = 1,
            Cpu = new Cpu
            {
                Id = 1,
                Brand = "Intel",
                Model = "Core i5",
                CreatedAt = DateTime.Now.ToString(),
            },
            DisplayId = 1,
            Display = new Display
            {
                Id = 1,
                Size = 15.6,
                ResolutionWidth = 1920,
                ResolutionHeight = 1080,
                PanelType = "IPS",
                CreatedAt = DateTime.Now.ToString(),
            },
            MemoryId = 1,
            Memory = new Memory
            {
                Id = 1,
                Capacity = 8,
                Speed = 3200,
                CreatedAt = DateTime.Now.ToString(),
            },
            StorageId = 1,
            Storage = new Storage
            {
                Id = 1,
                Capacity = 512,
                Type = "SSD",
                CreatedAt = DateTime.Now.ToString(),
            },
            Color = "Black",
            CreatedAt = DateTime.Now.ToString(),
            CreatedBy = null
        };

        Mock.Get(context.UserService)
            .Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new UserDto
            {
                Id = "1",
                Name = "User 1",
                Email = "",
                Password = "",
                Role = "Admin"
            });

        Mock.Get(context.PermissionService)
            .Setup(x => x.CanUpdateNotebook(
                It.IsAny<Notebook>(),
                It.IsAny<UserDto>()
            ))
            .Returns(true);

        context.DbContext.Add(notebook);
        context.DbContext.SaveChanges();

        // Act
        var notebookDto = new NotebookDto
        {
            Id = notebook.Id,
            BrandId = notebook.BrandId,
            ModelId = notebook.ModelId,
            CpuId = notebook.CpuId,
            DisplayId = notebook.DisplayId,
            MemoryId = notebook.MemoryId,
            StorageId = notebook.StorageId,
            Price = notebook.Price,
            Color = "White"
        };

        var result = await sut.Update(notebookDto);
        var updatedNotebook = context.DbContext.Notebooks.FirstOrDefault(n => n.Id == notebook.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, "Update should return true");
            Assert.That(updatedNotebook?.Id, Is.EqualTo(notebook.Id), "Notebook id should be the same");
            Assert.That(updatedNotebook?.BrandId, Is.EqualTo(notebook.BrandId), "Notebook brand id should be the same");
            Assert.That(updatedNotebook?.Color, Is.EqualTo("White"), "Notebook color should be updated");
        });
    }

    [Test]
    public async Task Delete_ReturnsNotebook()
    {
        using var context = TestStartup.CreateComponentsContext();

        var sut = context.Resolve<NotebookService>();

        // Arrange
        var notebook = new Notebook
        {
            Id = 1,
            BrandId = 1,
            Brand = new Brand
            {
                Id = 1,
                Name = "Brand 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            ModelId = 1,
            Model = new Model
            {
                Id = 1,
                Name = "Model 1",
                CreatedAt = DateTime.Now.ToString(),
            },
            CpuId = 1,
            Cpu = new Cpu
            {
                Id = 1,
                Brand = "Intel",
                Model = "Core i5",
                CreatedAt = DateTime.Now.ToString(),
            },
            DisplayId = 1,
            Display = new Display
            {
                Id = 1,
                Size = 15.6,
                ResolutionWidth = 1920,
                ResolutionHeight = 1080,
                PanelType = "IPS",
                CreatedAt = DateTime.Now.ToString(),
            },
            MemoryId = 1,
            Memory = new Memory
            {
                Id = 1,
                Capacity = 8,
                Speed = 3200,
                CreatedAt = DateTime.Now.ToString(),
            },
            StorageId = 1,
            Storage = new Storage
            {
                Id = 1,
                Capacity = 512,
                Type = "SSD",
                CreatedAt = DateTime.Now.ToString(),
            },
            Color = "Black",
            CreatedAt = DateTime.Now.ToString(),
            CreatedBy = null
        };

        Mock.Get(context.UserService)
            .Setup(x => x.GetCurrentUser())
            .ReturnsAsync(new UserDto
            {
                Id = "1",
                Name = "User 1",
                Email = "",
                Password = "",
                Role = "Admin"
            });

        Mock.Get(context.PermissionService)
            .Setup(x => x.CanUpdateNotebook(
                It.IsAny<Notebook>(),
                It.IsAny<UserDto>()
            ))
            .Returns(true);

        context.DbContext.Add(notebook);
        context.DbContext.SaveChanges();

        // Act
        var result = await sut.Delete(notebook.Id);
        var deletedNotebook = context.DbContext.Notebooks.FirstOrDefault(n => n.Id == notebook.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True, "Delete should return true");
            Assert.That(deletedNotebook, Is.Null, "Notebook should be deleted");
        });
    }
}
