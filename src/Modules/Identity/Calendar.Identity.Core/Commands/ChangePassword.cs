﻿using Calendar.Shared.Abstractions.Cqrs;

namespace Calendar.Identity.Core.Commands;

internal record ChangePassword(string OldPassword, string NewPassword) : ICommand;
