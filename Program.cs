using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI;
using MinimalAPI.Data;
using MinimalAPI.DTO;
using MinimalAPI.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/api/coupon", (ILogger <Program> _logger) => {

    APIResponse response = new();

    _logger.Log(LogLevel.Information, "Getting all Coupons");

    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);

}).WithName("GetCoupons").Produces<APIResponse>(200);


app.MapGet("/api/coupon/{id:int}", (ILogger<Program> _logger, int id) =>
{

    APIResponse response = new();

    _logger.Log(LogLevel.Information, "Getting specific ID Coupon");

    response.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);

}).WithName("GetCoupon").Produces<APIResponse>(200);

app.MapPost("/api/coupon/", async (ILogger<Program> _logger, IMapper _mapper, IValidator <CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_c_DTO)  => {

    APIResponse response = new() {IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    _logger.Log(LogLevel.Information, "Adding specific Coupon");

    var validationResult = await _validation.ValidateAsync(coupon_c_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }
    if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == coupon_c_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Name already exists");
        return Results.BadRequest(response);
    }

   Coupon coupon = _mapper.Map<Coupon>(coupon_c_DTO);

    coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);

    //return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, coupon);

    response.Result = coupon_c_DTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;

    return Results.Ok(response);


}).WithName("CreateCoupon").Produces<Coupon>(201).Produces(400);

app.MapPut("/api/coupon/", async (ILogger<Program> _logger, IMapper _mapper, IValidator<CouponPutDTO> _validation, [FromBody] CouponPutDTO coupon_p_DTO) => {

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    _logger.Log(LogLevel.Information, "Updating specific Coupon");

    var validationResult = await _validation.ValidateAsync(coupon_p_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == coupon_p_DTO.Id);
    couponFromStore.IsActive = coupon_p_DTO.IsActive;
    couponFromStore.Name = coupon_p_DTO.Name;
    couponFromStore.Percent = coupon_p_DTO.Percent;
    couponFromStore.LastUpdated = DateTime.Now;


    response.Result = coupon_p_DTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;

    return Results.Ok(response);

}).WithName("UpdateCoupon").Produces<CouponPutDTO>(200).Produces(400);


app.MapDelete("/api/coupon/", (ILogger<Program> _logger, int id) =>
{

    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    _logger.Log(LogLevel.Information, "Deleting specific Coupon");

    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == id);

    if (couponFromStore != null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }



}).WithName("DeleteCoupon").Produces<CouponPutDTO>(200).Produces(400);


app.UseHttpsRedirection();
app.Run();



