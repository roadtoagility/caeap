// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Stock.Capabilities;

public class Result<TSucceded, TFailed>
{
    public static Result<TSucceded, TFailed> FailedFor(TFailed value) =>
        new Result<TSucceded, TFailed>{IsSucceded = true,Failed = value};
    
    public static Result<TSucceded, TFailed> SucceedFor(TSucceded value) =>
        new Result<TSucceded, TFailed>{IsSucceded = true,Succeded = value};

    public bool IsSucceded { get; private set; } 
    public TSucceded Succeded { get; private set; }
    public TFailed Failed { get; private set; }
}
