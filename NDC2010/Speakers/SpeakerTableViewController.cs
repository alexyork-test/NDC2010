using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using NDC2010.Model;
using NDC2010.Logic.Presenters;

namespace NDC2010
{
	[Register("SpeakerTableViewController")]
	public partial class SpeakerTableViewController : UITableViewController
    {
		private static NSString CELL_ID = new NSString("SpeakerTableCell");
		private static NSString CELL_WITH_DETAIL_ID = new NSString("SpeakerTableCell_WithDetail");
		
		protected SpeakerPresenter Presenter;
	
		public SpeakerTableViewController(Speaker speaker) : base(UITableViewStyle.Grouped)
		{
			Presenter = new SpeakerPresenter();
			Presenter.Speaker = speaker;
			Presenter.Sessions = (UIApplication.SharedApplication.Delegate as AppDelegate).Sessions;
		}
        
		class TableSource : NDC2010DetailsTableViewSource
		{
			private SpeakerTableViewController _tvc;
			private Session[] _sessions;
		
			public TableSource(SpeakerTableViewController tvc)
			{
				_tvc = tvc;
				_sessions = _tvc.Presenter.GetSessions();
			}
		
			public override int RowsInSection(UITableView tableView, int section)
			{
				if (section == 2)
					return _sessions.Length;
				return 1;
			}
			
			public override int NumberOfSections(UITableView tableView)
			{
				return 3;
			}
			
			public override string TitleForHeader(UITableView tableView, int section)
			{
				switch (section)
				{
					case 0: return _tvc.Presenter.GetHeadingTextForName();
					case 1: return _tvc.Presenter.GetHeadingTextForBio();
					case 2: return _tvc.Presenter.GetHeadingTextForSessions();
				}
				return "";
			}
	
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell;
				
				if (CellHasDetailsLabel(indexPath))
				{
					cell = DequeueOrCreateTableCell(tableView, indexPath, CELL_WITH_DETAIL_ID, UITableViewCellStyle.Subtitle);
					cell.TextLabel.Font = NDC2010Fonts.TitleFont;
					cell.TextLabel.Text = _sessions.ElementAt(indexPath.Row).Title;
					cell.DetailTextLabel.Text = _sessions.ElementAt(indexPath.Row).Time;
				}
				else
				{
					cell = DequeueOrCreateTableCell(tableView, indexPath, CELL_ID);
					cell.TextLabel.Text = GetCellText(indexPath);
				}
				
				return cell;
			}
			
			public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return CellHasDetailsLabel(indexPath)
					? 44f
					: GetCellHeightForRow(GetCellText(indexPath), tableView,indexPath);
			}
			
			private string GetCellText(NSIndexPath indexPath)
			{
				switch (indexPath.Section)
				{
					case 0: return _tvc.Presenter.Speaker.Name;
					case 1: return _tvc.Presenter.Speaker.Info;
					case 2: return _sessions.ElementAt(indexPath.Row).Title;
				}
				return "";
			}
			
			private bool CellHasDetailsLabel(NSIndexPath indexPath)
			{
				return (indexPath.Section == 2);
			}
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			Title = Presenter.GetTitle();
			View.BackgroundColor = UIColor.Clear;
			TableView.Source = new TableSource(this);
		}
	}
}