using System.ComponentModel.DataAnnotations;

namespace NetEnhancements.Shared.Settings
{
#pragma warning disable CS8618 // Configuration system validates non-nullable properties according to their attributes except it doesn't.
    /// <summary>
    /// Configures the MassTransit RabbitMQ server settings.
    /// </summary>
    public class MessageQueueSettings
    {
        [Required]
        public string Server { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password {  get; set; }

        public string? ResponseQueueName { get; set; }
    }
#pragma warning restore CS8618
}
