using LicenseRentalWebApp.Models;
namespace LicenseRentalWebApp.Pages;

public static class LicenseEndpoints
{
    public static void MapLicenseEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/License", () =>
        {
            return new [] { new License() };
        })
        .WithName("GetAllLicenses")
        .Produces<License[]>(StatusCodes.Status200OK);

        routes.MapGet("/api/License/{id}", (int id) =>
        {
            //return new License { ID = id };
        })
        .WithName("GetLicenseById")
        .Produces<License>(StatusCodes.Status200OK);

        routes.MapPut("/api/License/{id}", (int id, License input) =>
        {
            return Results.NoContent();
        })
        .WithName("UpdateLicense")
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/License/", (License model) =>
        {
            //return Results.Created($"/Licenses/{model.ID}", model);
        })
        .WithName("CreateLicense")
        .Produces<License>(StatusCodes.Status201Created);

        routes.MapDelete("/api/License/{id}", (int id) =>
        {
            //return Results.Ok(new License { ID = id });
        })
        .WithName("DeleteLicense")
        .Produces<License>(StatusCodes.Status200OK);
    }
}
