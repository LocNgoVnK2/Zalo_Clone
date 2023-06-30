using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repository;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zalo_Clone;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ZaloDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddSingleton(new MapperConfiguration(mc =>
{
    mc.AddProfile(new Mapping());
}).CreateMapper());

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<ZaloDbContext>().AddDefaultTokenProviders();
#region Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//Role
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
//Reaction

builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IReactionService, ReactionService>();

//UserData
builder.Services.AddScoped<IUserDataRepository, UserDataRepository>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
//block
builder.Services.AddScoped<IBlockListRepository, BlockListRepository>();
builder.Services.AddScoped<IBlockService, BlockService>();
//friend request
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
//friend list
builder.Services.AddScoped<IFriendListRepository, FriendListRepository>();
builder.Services.AddScoped<IFriendListService, FriendListService>();
//Message
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageReceipentRepository, MessageReceipentRepository>();
builder.Services.AddScoped<IMessageAttachmentRepository, MessageAttachmentRepository>();
builder.Services.AddScoped<IMessageToDoListRepository, MessageToDoListRepository>();
builder.Services.AddScoped<IMessageGroupRepository, MessageGroupRepository>();
builder.Services.AddScoped<IMessageReactDetailRepository, MessageReactDetailRepository>();
//Group 
builder.Services.AddScoped<IGroupRoleRepository, GroupRoleRepository>();
builder.Services.AddScoped<IGroupRoleService, GroupRoleService>();
builder.Services.AddScoped<IGroupChatRepository, GroupChatRepository>();
builder.Services.AddScoped<IGroupChatService, GroupChatService>();
builder.Services.AddScoped<IGroupUserRepository, GroupUserRepository>();
builder.Services.AddScoped<IGroupUserService, GroupUserService>();
//To do list
builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
builder.Services.AddScoped<IToDoListService, ToDoListService>();
builder.Services.AddScoped<IToDoUserRepository, ToDoUserRepository>();
builder.Services.AddScoped<IToDoUserService, ToDoUserService>();
// mute 
builder.Services.AddScoped<IMuteService, MuteService>();
builder.Services.AddScoped<IMuteGroupRepository, MuteGroupRepository>();
builder.Services.AddScoped<IMuteUserRepository, MuteUserRepository>();
// user role
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))

    };
});
// user account
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();