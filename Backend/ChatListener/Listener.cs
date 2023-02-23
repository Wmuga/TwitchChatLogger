using Backend.Context;
using Backend.Models;
using Backend.Service;
using Channel = Backend.Models.Channel;

namespace Backend.ChatListener
{
    public interface IListener
    {
        public Task<int> Join(string channelName);
        public void Part(string channel);

	}
    public class Listener : IListener
    {
        readonly TwitchIrcClient _client;
        readonly HashSet<string> _channels = new();
		readonly IServiceProvider _serviceProvider;
        readonly ILoggerService _logger;

        private Listener() {
            throw new NotImplementedException();
        }

        public Listener(IServiceProvider serviceProvider, ILoggerService logger, ITwitchIrcClient client)
        {
			_serviceProvider = serviceProvider;
            _logger = logger;
            _logger.Log("Init logger");
            _client = (TwitchIrcClient)client;
            _client.OnMessage += Client_OnChat;
            _client.Connect();
        }

        private void Client_OnChat(object sender, MsgEventArgs msg)
        {
			using var scope = _serviceProvider.CreateScope();
            var _dbservice = (DBService)scope.ServiceProvider.GetRequiredService<IDbService>();
			long userId = _dbservice.GetOrCreateUser( msg.User).GetAwaiter().GetResult().Id;
			long channelId = _dbservice.GetOrCreateChannel( msg.Channel).GetAwaiter().GetResult().Id;
			_dbservice.AddMessage( new Message
			{
				UserId = userId,
				ChannelId = channelId,
				Color = msg.Tags.Color,
				Content = msg.Message,
				MsgType = msg.MessageType,
				Timestamp = msg.Time
			}).GetAwaiter().GetResult();
		}

        public async Task<int> Join(string channelName)
        {
            if (!channelName.StartsWith('#')) channelName = '#' + channelName;
            channelName = channelName.ToLower();
			using var scope = _serviceProvider.CreateScope();
			var _dbservice = scope.ServiceProvider.GetRequiredService<IDbService>();
			Channel channel = await _dbservice.GetOrCreateChannel(channelName);
            _channels.Add(channel.Name);
            _client.Join(channelName);
			_logger.Log($"Joined channel {channelName}, {channel.Name}");
            return 0;
        }
        public void Part(string channelName) {
			if (!channelName.StartsWith('#')) channelName = '#' + channelName;
			_logger.Log($"Left channel {channelName}");
			_channels.Remove(channelName);
            _client.Part(channelName);
        }
    }
}
