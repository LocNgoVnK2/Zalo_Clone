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

            //Contact
            CreateMap<Contact, UserContactModel>();
            CreateMap<UserContactModel, Contact>();
            CreateMap<Contact, ContactDataModel>();
            CreateMap<ContactDataModel, Contact>();
            //
            CreateMap<SignUpModel, User>();
            CreateMap<User, SignUpModel>();

            CreateMap<SignInModel, User>();
            CreateMap<User, SignInModel>();

            CreateMap<SignUpModel, SignUpUser>();
            CreateMap<SignUpUser, SignUpModel>();

            //Block
            CreateMap<BlockList, BlockModel>();
            CreateMap<BlockModel, BlockList>();
            //User

            //Message
            CreateMap<Message, MessageContactModel>();
            CreateMap<MessageContactModel, Message>();
            CreateMap<MessageSendModel, Message>();
            CreateMap<MessageAttachment, MessageAttachmentModel>();
            CreateMap<MessageAttachmentModel, MessageAttachment>();
            //GroupRole
            CreateMap<GroupRole, GroupRoleModel>();
            CreateMap<GroupRoleModel, GroupRole>();

            CreateMap<GroupChat, GroupChatModel>();
            CreateMap<GroupChatModel, GroupChat>();


            CreateMap<GroupUser, GroupUserModel>();
            CreateMap<GroupUserModel, GroupUser>();

            CreateMap<Message, MessageContactModel>();
            CreateMap<MessageContactModel, Message>();

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

            // validation
            CreateMap<ValidationByEmailModel, ValidationByEmail>();
            CreateMap<ValidationByEmail, ValidationByEmailModel>();
            // friendRequest

            CreateMap<FriendRequestModel, FriendRequest>();
            CreateMap<FriendRequest, FriendRequestModel>();
        }
    }
}
