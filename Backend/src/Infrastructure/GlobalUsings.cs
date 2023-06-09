﻿global using Application.Common.Helper;
global using Application.Common.Interfaces;
global using Application.Common.Models;
global using Application.Common.Models.Request;
global using Application.Follows.Shared.Interfaces;
global using Application.IdentityManagement.Auth.Commands.RefreshToken;
global using Application.IdentityManagement.Auth.Commands.ResetPassword;
global using Application.IdentityManagement.Auth.Commands.SignIn;
global using Application.IdentityManagement.Auth.Commands.SignUp;
global using Application.IdentityManagement.Shared.Interfaces;
global using Application.IdentityManagement.Shared.Models;
global using Application.Tweets.Shared.Interfaces;
global using AutoMapper;
global using Domain.Entities;
global using Infrastructure.Configurations;
global using Infrastructure.Identity;
global using Infrastructure.MessageQueueServices.RabbitMQ;
global using Infrastructure.Persistence.MongoDB.Common;
global using Infrastructure.Persistence.MongoDB.DbContext;
global using Infrastructure.Persistence.MongoDB.Repositories;
//global using Infrastructure.Persistence.RedisCaching;
global using Infrastructure.Repositories;
global using Infrastructure.Services;
global using MailKit.Net.Smtp;
global using MediatR;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Tokens;
global using MimeKit;
global using MimeKit.Text;
global using MongoDB.Driver;
global using System.IdentityModel.Tokens.Jwt;
global using System.Linq.Expressions;
global using System.Security.Claims;
global using System.Text;
