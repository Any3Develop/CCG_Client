﻿namespace Client.Services.UIService.FullFade
{
	public interface IUIFullFadePresenter
	{
		void Init(IUIService uiService);
		void OnDeleted(IUIWindow window);
		void OnShow(IUIWindow window);
		void OnHidden(IUIWindow window);
	}
}