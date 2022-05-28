using webapi;
using webapi.Controllers;
using webapi.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace webapi.Tests;

public class FileControllerUnitTest
{
    public List<CustomFile> GetTestData()
    {
        return new()
        {
            new CustomFile{ID = 0, Name = "text.txt", Size = 2000, Bytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }},
            new CustomFile{ID = 0, Name = "importantResume.pdf", Size = 10000, Bytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }},
            new CustomFile{ID = 0, Name = "archive.rar", Size = 30100, Bytes = new byte[30100]},
            new CustomFile{ID = 0, Name = "cat.png", Size = 2022, Bytes = new byte[2022]},
        };
    }

    [Fact]
    public async void GetFileListShouldReturnList()
    {
        var testData = GetTestData();

        var options = new DbContextOptionsBuilder<FileContext>()
            .UseInMemoryDatabase(databaseName: "GetFileListShouldReturnList")
            .Options;

        using (var context = new FileContext(options))
        {

            await context.CustomFiles.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        using (var context = new FileContext(options))
        {
            var service = new FileService(context);

            var fileController = new FileController(null, service);
            var recievedList = service.GetFileList();


            Assert.Equal(4, recievedList.Count());
            foreach (var testCase in testData.Zip(recievedList, Tuple.Create))
            {
                Assert.Equal(testCase.Item1.ID, testCase.Item2.ID);
                Assert.Equal(testCase.Item1.Name, testCase.Item2.Name);
                Assert.Equal(testCase.Item1.Size, testCase.Item2.Size);
                Assert.Null(testCase.Item2.Bytes);
            }

        }
    }

    [Fact]
    public async void GetFileReturnsFile()
    {
        var testData = GetTestData();

        var options = new DbContextOptionsBuilder<FileContext>()
            .UseInMemoryDatabase(databaseName: "GetFileReturnsFile")
            .Options;

        using (var context = new FileContext(options))
        {

            await context.CustomFiles.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        using (var context = new FileContext(options))
        {
            var service = new FileService(context);

            var recievedCustomFile = await service.GetFileById(2);
            var testCustomFile = testData[1];


            Assert.True(testCustomFile.Equals(recievedCustomFile));
        }


    }

    [Fact]
    public async void UploadFileUploadsToDB()
    {
        var testData = GetTestData();

        var options = new DbContextOptionsBuilder<FileContext>()
            .UseInMemoryDatabase(databaseName: "UploadFileUploadsToDB")
            .Options;

        using (var context = new FileContext(options))
        {

            await context.CustomFiles.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        using (var context = new FileContext(options))
        {
            var service = new FileService(context);

            var testCustomFile = new CustomFile { ID = 0, Name = "johnCena.mp3", Size = 10, Bytes = new byte[10] };

            Stream stream = new MemoryStream(testCustomFile.Bytes);

            var formFile = new FormFile(stream, 0, stream.Length, testCustomFile.Name, testCustomFile.Name);

            await service.UploadFile(formFile);
            var recievedCustomFile = await service.GetFileById(context.CustomFiles.Count());
            testCustomFile.ID = recievedCustomFile.ID;


            Assert.Equal(5, context.CustomFiles.Count());
            Assert.True(testCustomFile.Equals(recievedCustomFile));
        }


    }

    [Fact]
    public async void DeletesFile()
    {
        var testData = GetTestData();

        var options = new DbContextOptionsBuilder<FileContext>()
            .UseInMemoryDatabase(databaseName: "DeletesFile")
            .Options;

        using (var context = new FileContext(options))
        {
            await context.CustomFiles.AddRangeAsync(testData);
            await context.SaveChangesAsync();
        }

        using (var context = new FileContext(options))
        {
            var service = new FileService(context);

            await service.DeleteFile(2);

            Assert.Equal(3, context.CustomFiles.Count());
        }


    }

}