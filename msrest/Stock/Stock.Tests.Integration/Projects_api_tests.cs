using System;
using System.Threading.Tasks;
using Ecommerce.Tests.Integration.Support;
using Xunit;

namespace Ecommerce.Tests.Integration
{
    public class Projects_api_tests : IClassFixture<EcommerceSUT<Program>>
    {
        private readonly EcommerceSUT<Program> _factory;

        public Projects_api_tests(EcommerceSUT<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Post_NewProject()
        {
            // Arrange
            var url = "/api/projects/save";
            // var command =  new AddProjectCommand(
            //     "S20210209O125478593",
            //     "doug.ramalho@gma.com",
            //     "PojectFake",
            //     DateTime.Now,
            //     134,
            //     Guid.NewGuid(), 
            //     "23234234",
            //     true,
            //     "ToApprove");
            //
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost/")
            // });
            //
            // // Act
            // var response = await client.PostAsJsonAsync(url, command);

            // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var result = await response.Content.ReadFromJsonAsync<CommandResult<Guid>>();
            //
            // Assert.True(result?.IsSucceed);
            // Assert.False(result?.Id.Equals(Guid.Empty));
        }

        [Theory]
        [InlineData("/api/projects/save/", "7D74E1C4-3C35-47B9-B17B-7D5F9D9DFCF6")]
        public async Task Put_UpdateProjectDetails(string url, Guid id)
        {
            // // Arrange
            // var command = new UpdateProjectCommand(   
            //     Guid.NewGuid(),
            //     "PojectFake",
            //     134,
            //     "ornwer11@ok.com",
            //     "23234234",
            //     "ToApprove");
            //
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost/")
            // });
            //
            // // Act
            // var response = await client.PutAsJsonAsync(string.Concat(url, id), command);

            // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var result = await response.Content.ReadFromJsonAsync<ExecutionResult>();
            //
            // Assert.True(result?.IsSucceed);
        }

        [Theory]
        [InlineData("/api/projects/list/{0}/", "AA8CD061-B3C0-4931-9272-0D7A9014B616")]
        public async Task Get_Projects_ByClient(string url, Guid id)
        {
            // // Arrange
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost")
            // });
            //
            // // Act
            // var response = await client.GetAsync(string.Format(url, id));
            //
            // // Assert
            //
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var data = await response.Content.ReadAsStringAsync();
            // var projects = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetProjectsResponse>(data));
            // Assert.True(projects?.IsSucceed);
            // Assert.Equal(2, projects?.Data.Count);
        }

        [Theory]
        [InlineData("/api/projects/list/{0}/{1}", "AA8CD061-B3C0-4931-9272-0D7A9014B616", "my-project")]
        public async Task Get_Projects_ByClient_Name(string url, Guid id, string name)
        {
            // // Arrange
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost")
            // });
            //
            // // Act
            // var response = await client.GetAsync(string.Format(url, id, name));
            //
            // // Assert
            //
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var data = await response.Content.ReadAsStringAsync();
            // var projects = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetProjectsResponse>(data));
            // Assert.True(projects?.IsSucceed);
            // Assert.Equal(1, projects?.Data.Count);
        }

        [Theory]
        [InlineData("/api/projects/", "DEF2A92E-A53E-4754-B811-2C0C7C7858FA")]
        [InlineData("/api/projects/", "11263366-D54A-4E7A-B820-D8894B6C5362")]
        public async Task Get_Project_By_Id(string url, Guid id)
        {
            // // Arrange
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost")
            // });
            //
            // // Act
            // var response = await client.GetAsync(string.Concat(url, id));
            //
            // // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var data = await response.Content.ReadAsStringAsync();
            // var found = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetProjectResponse>(data));
            // Assert.True(found?.IsSucceed);
            // Assert.Equal(id, found?.Data.Id);
        }

        [Theory]
        [InlineData("/api/projects/{0}", "41557BC9-1809-4B0F-B8E7-4F61CC06D2C9")]
        public async Task Delete_Project(string url, Guid id)
        {
            // // Arrange
            // var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            // {
            //     AllowAutoRedirect = true,
            //     BaseAddress = new Uri("https://localhost")
            // });
            //
            // // Act
            // var response = await client.DeleteAsync(string.Format(url, id));
            //
            // // Assert
            // response.EnsureSuccessStatusCode(); // Status Code 200-299
            // var data = await response.Content.ReadAsStringAsync();
            // var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ExecutionResult>(data));
            // Assert.True(result?.IsSucceed);
        }
    }
}