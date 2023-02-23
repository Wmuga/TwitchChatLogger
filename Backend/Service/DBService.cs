using Backend.Context;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public interface IDbService
    {
        public Task<User?> GetUserById(long id);
        public Task<User?> GetUserByName(string username);
        public Task<User> GetOrCreateUser(string username);

        public Task<Channel?> GetChannelById(long id);
        public Task<Channel?> GetChannelByName(string channelName);

        public Task<Channel> GetOrCreateChannel(string channel);

        public Task<User> AddUser(User user);

        public Task<Channel> AddChannel(Channel channel);

        public Task<Message> AddMessage(Message msg);

        public long GetNextUserId();
        public long GetNextChannelId();
        public long GetNextMessageId();
	}
    public class DBService : IDbService
    {
        readonly ChatContext _context;
        public DBService(ChatContext context) { 
            _context= context;
        }

        public async Task<User?> GetUserById( long id) => await _context.Users.FindAsync(id);
        public async Task<User?> GetUserByName(  string username) => await _context.Users.Where(user => user.Name == username).FirstOrDefaultAsync();

        public async Task<User> GetOrCreateUser( string username) => (await GetUserByName(username)) ?? (await AddUser(new User { Name = username }));

        public async Task<Channel?> GetChannelById( long id) => await _context.Channels.FindAsync(id);
        public async Task<Channel?> GetChannelByName( string channelName) => await _context.Channels.Where(channel => channel.Name == channelName).FirstOrDefaultAsync();

        public async Task<Channel> GetOrCreateChannel( string channel) => (await GetChannelByName(channel)) ?? (await AddChannel(new Channel { Name = channel }));

        public async Task<User> AddUser( User user)
        {
            user.Id = GetNextUserId();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Channel> AddChannel( Channel channel)
        {
            channel.Id = GetNextChannelId();
            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();
            return channel;
        }

        public async Task<Message> AddMessage( Message msg)
        {
            msg.Color ??= "#000000";
			msg.Id = GetNextMessageId();
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();
            return msg;
        }

        public long GetNextUserId() => _context.Users.Any() ? _context.Users.OrderBy(e=>e.Id).Last().Id + 1 : 1;
        public long GetNextChannelId() => _context.Channels.Any() ? _context.Channels.OrderBy(e => e.Id).Last().Id + 1 : 1;
        public long GetNextMessageId() => _context.Messages.Any() ? _context.Messages.OrderBy(e => e.Id).Last().Id + 1 : 1;
    }
}
