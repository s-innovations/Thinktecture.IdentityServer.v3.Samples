﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thinktecture.IdentityManager;
using Thinktecture.IdentityManager.MembershipReboot;

namespace Thinktecture.IdentityManager.Host
{
    public class MembershipRebootIdentityManagerFactory
    {
        static MembershipRebootConfiguration<RelationalUserAccount> config;
        static MembershipRebootIdentityManagerFactory()
        {
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.MigrateDatabaseToLatestVersion<DefaultMembershipRebootDatabase, BrockAllen.MembershipReboot.Ef.Migrations.Configuration>());

            config = new MembershipRebootConfiguration<RelationalUserAccount>();
            config.PasswordHashingIterationCount = 10000;
            config.RequireAccountVerification = false;
        }

        string connString;
        public MembershipRebootIdentityManagerFactory(string connString)
        {
            this.connString = connString;
        }
        
        public IIdentityManagerService Create()
        {
            var userrepo = new DefaultUserAccountRepository(this.connString);
            userrepo.QueryFilter = RelationalUserAccountQuery.Filter;
            userrepo.QuerySort = RelationalUserAccountQuery.Sort;
            var usersvc = new UserAccountService<RelationalUserAccount>(config, userrepo);
            
            var grprepo = new DefaultGroupRepository(this.connString);
            var grpsvc = new GroupService<RelationalGroup>(config.DefaultTenant, grprepo);
            
            var svc = new MembershipRebootIdentityManagerService<RelationalUserAccount, RelationalGroup>(usersvc, userrepo, grpsvc, grprepo);
            return new DisposableIdentityManagerService(svc, userrepo);
        }
    }
}