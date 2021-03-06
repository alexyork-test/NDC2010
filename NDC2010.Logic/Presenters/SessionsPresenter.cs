using System;
using System.Collections.Generic;
using System.Linq;
using NDC2010.Model;

namespace NDC2010.Logic.Presenters
{
	public class SessionsPresenter
	{
		protected List<Session> Sessions { get; set; }
		protected int Day { get; set; }
		
		public SessionsPresenter(List<Session> sessions, int day)
		{
			Sessions = sessions;
			Day = day;
		}
		
		public List<Session> GetSessionsForDay(int day)
		{
			var query = from session in Sessions
						where session.Day == day
						select session;
			
			return query.ToList();
		}
			
		// TODO: rename and refactor this: the presenters shouldnt be aware of "sections"
		public string GetTimeForSection(int section)
		{
			switch (section)
			{
				case 0: return "9:00 - 10:00";
				case 1: return "10:20 - 11:20";
				case 2: return "11:40 - 12:40";
				case 3: return "13:40 - 14:40";
				case 4: return "15:00 - 16:00";
				case 5: return "16:20 - 17:20";
				case 6: return "17:40 - 18:40";
				default: return "";
			}
		}
		
		// TODO: rename and refactor this: the presenters shouldnt be aware of "sections"
		public List<Session> GetSessionsForSection(int section)
		{
			var query = from session in Sessions
						where session.Day == Day &&
							  session.Time == GetTimeForSection(section)
						select session;
			
			return query.ToList();
		}
		
		public int GetNumberOfDailyTimeslots()
		{
			if (Day == 3) return 6;
			return 7;
		}
		
		public string GetTitle()
		{
			return "Sessions, Day " + Day;
		}
	}
}