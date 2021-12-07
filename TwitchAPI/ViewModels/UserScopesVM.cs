using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Models;

namespace TwitchAPI.ViewModels
{
    public enum ScopeCategory
    {
        analytics,
        bits,
        channel,
        clips,
        moderator,
        user,
    }

    public class ScopeContainer
    {
        public List<CheckBox> CheckBoxes = new List<CheckBox>
    {
        new CheckBox{Text = Scope.analytics_read_extensions, Value = "analytics:read:extensions",Category = ScopeCategory.analytics},
        new CheckBox{Text = Scope.analytics_read_games, Value = "analytics:read:games", Category = ScopeCategory.analytics},
        new CheckBox{Text = Scope.bits_read, Value = "bits:read", Category = ScopeCategory.bits},
        new CheckBox{Text = Scope.channel_edit_commercial, Value = "channel:edit:commercial", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_broadcast, Value = "channel:manage:broadcast", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_extensions, Value = "channel:manage:extensions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_polls, Value = "channel:manage:polls", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_predictions, Value = "channel:manage:predictions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_redemptions, Value = "channel:manage:redemptions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_schedule, Value = "channel:manage:schedule", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_manage_videos, Value = "channel:manage:videos", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_editors, Value = "channel:read:editors", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_goals, Value = "channel:read:goals", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_hype_train, Value = "channel:read:hype_train", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_polls, Value = "channel:read:polls", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_predictions, Value = "channel:read:predictions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_redemptions, Value = "channel:read:redemptions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_stream_key, Value = "channel:read:stream_key", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.channel_read_subscriptions, Value = "channel:read:subscriptions", Category = ScopeCategory.channel},
        new CheckBox{Text = Scope.clips_edit, Value = "clips:edit", Category = ScopeCategory.clips},
        new CheckBox{Text = Scope.moderation_read, Value = "moderation:read", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_manage_automod, Value = "moderator:manage:banned_users", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_manage_automod_settings, Value = "moderator:read:blocked_terms", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_manage_banned_users, Value = "moderator:manage:blocked_terms", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_manage_blocked_terms, Value = "moderator:manage:automod", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_manage_chat_settings, Value = "moderator:read:automod_settings", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_read_automod_settings, Value = "moderator:manage:automod_settings", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_read_blocked_terms, Value = "moderator:read:chat_settings", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.moderator_read_chat_settings, Value = "moderator:manage:chat_settings", Category = ScopeCategory.moderator},
        new CheckBox{Text = Scope.user_edit, Value = "user:edit", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_edit_follows, Value = "user:edit:follows", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_manage_blocked_users, Value = "user:manage:blocked_users", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_read_blocked_users, Value = "user:read:blocked_users", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_read_broadcast, Value = "user:read:broadcast", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_read_email, Value = "user:read:email", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_read_follows, Value = "user:read:follows", Category = ScopeCategory.user},
        new CheckBox{Text = Scope.user_read_subscriptions, Value = "user:read:subscriptions", Category = ScopeCategory.user},
    };

        public string ScopesFormatted { get; set; }
    }

    public class CheckBox
    {
        public bool PreviouslySelected = false;
        public string Value { get; set; }
        public Scope Text { get; set; }
        public ScopeCategory Category { get; set; }
    }
}
