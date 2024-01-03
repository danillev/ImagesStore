using ImagesStore.API.Services;

var builder = WebApplication.CreateBuilder(args);
var serviceFacade = new ServiceFacade(builder);
serviceFacade.Run();