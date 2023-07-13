using AutoMapper;
using Infrastructure.Entities;
using Zalo_Clone.Models;
using Zalo_Clone.ModelViews;

namespace Zalo_Clone
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Reaction
            CreateMap<ReactionModel, Reaction>();
            CreateMap<Reaction, ReactionModel>();

            //UserData
            CreateMap<UserDataModel, UserData>();
            CreateMap<UserData, UserDataModel>();


            CreateMap<SignUpModel, User>();
            CreateMap<User, SignUpModel>();

            CreateMap<SignInModel, User>();
            CreateMap<User, SignInModel>();

            //Block
            CreateMap<BlockList, BlockModel>();
            CreateMap<BlockModel, BlockList>();
            //User

            //Message
            CreateMap<Message, MessageReceipentModel>();
            CreateMap<MessageReceipentModel, Message > ();
            //GroupRole
            CreateMap<GroupRole, GroupRoleModel>();
            CreateMap<GroupRoleModel, GroupRole>();

            CreateMap<GroupChat, GroupChatModel>();
            CreateMap<GroupChatModel, GroupChat>();


            CreateMap<GroupUser, GroupUserModel>();
            CreateMap<GroupUserModel, GroupUser>();

            CreateMap<Message, MessageGroupModel>();
            CreateMap<MessageGroupModel, Message>();

            CreateMap<Message, MessageToDoListModel>();
            CreateMap<MessageToDoListModel, Message>();

            CreateMap<MessageReactDetail, MessageReactModel>();
            CreateMap<MessageReactModel, MessageReactDetail>();

            // ToDoList
            CreateMap<ToDoList, ToDoListModel>();
            CreateMap<ToDoListModel, ToDoList>();

            CreateMap<ToDoUser, ToDoUserModel>();
            CreateMap<ToDoUserModel, ToDoUser>();

            // Mute
            CreateMap<MuteGroup, MuteGroupModel>();
            CreateMap<MuteGroupModel, MuteGroup>();

            CreateMap<MuteUser, MuteUserModel>();
            CreateMap<MuteUserModel, MuteUser>();

            // userRole
            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>();
            CreateMap<UserRole, UserRoleModel>();
            CreateMap<UserRoleModel, UserRole>();
        }
    }
}
