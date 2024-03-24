﻿using CarRental.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace CarRental.Api.Filters
{
    public class ExceptionsAttribute : Attribute, IExceptionFilter
    {
        private static Dictionary<Type, HttpStatusCode> StatusCodeExceptionsDictionary = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(ClientNotFoundException), HttpStatusCode.NotFound },
            { typeof(RentalNotFoundException), HttpStatusCode.NotFound },
            { typeof(VehicleNotFoundException), HttpStatusCode.NotFound },
            { typeof(VehicleNotAvailableException), HttpStatusCode.Conflict },
            { typeof(Exception), HttpStatusCode.InternalServerError },
        };

        public void OnException(ExceptionContext context)
        {
            var errorDetail = new ErrorDetailModel();
            StatusCodeExceptionsDictionary.TryGetValue(context.Exception.GetType(), out var statusCode);
            errorDetail.State = statusCode.ToString();
            errorDetail.StatusCode = (int)statusCode;
            errorDetail.Detail = context.Exception.Message;
            errorDetail.Errors.Add(new Error
            {
                Title = context.Exception.Message,
                Detail = context.Exception.InnerException?.Message ?? string.Empty
            });

            context.Result = new ObjectResult(errorDetail);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
