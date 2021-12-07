using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Models
{
    public enum Scope
    {
        [Display(Name = "View analytics data for the Twitch Extensions owned by the authenticated account.")]
        analytics_read_extensions,

        [Display(Name = "View analytics data for the games owned by the authenticated account.")]
        analytics_read_games,

        [Display(Name = "View Bits information for a channel.")]
        bits_read,

        [Display(Name = "Run commercials on a channel.")]
        channel_edit_commercial,

        [Display(Name = "Manage a channel’s broadcast configuration, including updating channel configuration and managing stream markers and stream tags.")]
        channel_manage_broadcast,

        [Display(Name = "Manage a channel’s Extension configuration, including activating Extensions.")]
        channel_manage_extensions,

        [Display(Name = "Manage a channel’s polls.")]
        channel_manage_polls,

        [Display(Name = "Manage of channel’s Channel Points Predictions")]
        channel_manage_predictions,

        [Display(Name = "Manage Channel Points custom rewards and their redemptions on a channel.")]
        channel_manage_redemptions,

        [Display(Name = "Manage a channel’s stream schedule.")]
        channel_manage_schedule,

        [Display(Name = "Manage a channel’s videos, including deleting videos.")]
        channel_manage_videos,

        [Display(Name = "View a list of users with the editor role for a channel.")]
        channel_read_editors,

        [Display(Name = "View Creator Goals for a channel.")]
        channel_read_goals,

        [Display(Name = "View Hype Train information for a channel.")]
        channel_read_hype_train,

        [Display(Name = "View a channel’s polls.")]
        channel_read_polls,

        [Display(Name = "View a channel’s Channel Points Predictions.")]
        channel_read_predictions,

        [Display(Name = "View Channel Points custom rewards and their redemptions on a channel.")]
        channel_read_redemptions,

        [Display(Name = "View an authorized user’s stream key.")]
        channel_read_stream_key,

        [Display(Name = "View a list of all subscribers to a channel and check if a user is subscribed to a channel.")]
        channel_read_subscriptions,

        [Display(Name = "Manage Clips for a channel.")]
        clips_edit,

        [Display(Name = "View a channel’s moderation data including Moderators, Bans, Timeouts, and Automod settings.")]
        moderation_read,

        [Display(Name = "Ban and unban users.")]
        moderator_manage_banned_users,

        [Display(Name = "View a broadcaster’s list of blocked terms.")]
        moderator_read_blocked_terms,

        [Display(Name = "Manage a broadcaster’s list of blocked terms.")]
        moderator_manage_blocked_terms,

        [Display(Name = "Manage messages held for review by AutoMod in channels where you are a moderator.")]
        moderator_manage_automod,

        [Display(Name = "View a broadcaster’s AutoMod settings.")]
        moderator_read_automod_settings,

        [Display(Name = "Manage a broadcaster’s AutoMod settings.")]
        moderator_manage_automod_settings,

        [Display(Name = "View a broadcaster’s chat room settings.")]
        moderator_read_chat_settings,

        [Display(Name = "Manage a broadcaster’s chat room settings.")]
        moderator_manage_chat_settings,

        [Display(Name = "Manage a user object.")]
        user_edit,

        [Display(Name = "Deprecated. Was previously used for “Create User Follows” and “Delete User Follows.")]
        user_edit_follows,

        [Display(Name = "Manage the block list of a user.")]
        user_manage_blocked_users,

        [Display(Name = "View the block list of a user.")]
        user_read_blocked_users,

        [Display(Name = "View a user’s broadcasting configuration, including Extension configurations.")]
        user_read_broadcast,

        [Display(Name = "View a user’s email address.")]
        user_read_email,

        [Display(Name = "View the list of channels a user follows.")]
        user_read_follows,

        [Display(Name = "View if an authorized user is subscribed to specific channels.")]
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
        public string[] Scopes { get; set; }
    }
}
