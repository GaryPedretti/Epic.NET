//  
//  ObserverBaseQA.cs
//  
//  Author:
//       Giacomo Tesio <giacomo@tesio.it>
// 
//  Copyright (c) 2010-2011 Giacomo Tesio
// 
//  This file is part of Epic.NET.
// 
//  Epic.NET is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Epic.NET is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
// 
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  
using NUnit.Framework;
using System;
using Epic.Fakes;
using Challenge00.DDDSample.Cargo;

namespace Epic
{
    [TestFixture()]
    public class ObserverBaseQA
    {
		[Test]
		public void Initialize_withoutIdentityReader_throwsArgumentNullException()
		{
		    // assert:
            Assert.Throws<ArgumentNullException>(delegate
            { 
                new FakeObserver<ICargo, TrackingId>(null);
            });
		}
        
        [Test]
        public void Initialize_withIdentityReader_works()
        {
            // arrange:
            Func<ICargo, TrackingId> identityReader = c => c.TrackingId;

            // act:
            ObserverBase<ICargo, TrackingId> observer = new FakeObserver<ICargo, TrackingId>(identityReader);

            // assert:
            Assert.IsNotNull(observer);
        }
    }
}
