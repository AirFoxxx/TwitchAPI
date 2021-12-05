using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Models
{
    public enum Scope
    {
        [Display(Name = "Extensions")]
        analytics_read_extensions,

        [Display(Name = "Games")]
        analytics_read_games,

        bits_read,
        channel_edit_commercial,
        channel_manage_broadcast,
        channel_manage_extensions,
        channel_manage_polls,
        channel_manage_predictions,
        channel_manage_redemptions,
        channel_manage_schedule,
        channel_manage_videos,
        channel_read_editors,
        channel_read_goals,
        channel_read_hype_train,
        channel_read_polls,
        channel_read_predictions,
        channel_read_redemptions,
        channel_read_stream_key,
        channel_read_subscriptions,
        clips_edit,
        moderation_read,
        moderator_manage_banned_users,
        moderator_read_blocked_terms,
        moderator_manage_blocked_terms,
        moderator_manage_automod,
        moderator_read_automod_settings,
        moderator_manage_automod_settings,
        moderator_read_chat_settings,
        moderator_manage_chat_settings,
        user_edit,
        user_edit_follows,
        user_manage_blocked_users,
        user_read_blocked_users,
        user_read_broadcast,
        user_read_email,
        user_read_follows,
        user_read_subscriptions,
    }

    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(100)]
        public string OAuthCode { get; set; }

        public TimeSpan ExpiresIn { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserToken { get; set; }

        [MaxLength(100)]
        public string RefreshToken { get; set; }

        [Required]
        public ICollection<Scope> Scopes { get; set; }
    }
}
