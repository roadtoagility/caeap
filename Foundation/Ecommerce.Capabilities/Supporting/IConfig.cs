// Copyright (C) 2023  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using DFlow.Events;
using DFlow.Validation;

namespace Ecommerce.Capabilities.Supporting;

public interface IConfig
{
    Result<string, Failure> FromEnvironment(string configKey);
}