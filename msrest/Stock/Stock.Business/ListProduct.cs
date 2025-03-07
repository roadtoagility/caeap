// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace Stock.Business;

public record Paged(int pageNum, int pageSize);

public record ProductList(string Description, string Name, int pageNum, int pageSize)
    : Paged(pageNum, pageSize);