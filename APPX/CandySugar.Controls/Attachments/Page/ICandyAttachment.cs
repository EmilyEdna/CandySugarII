using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UraniumUI.Pages;

namespace CandySugar.Controls
{
    public interface ICandyAttachment: IView
    {
        void OnAttached(CandyUIPage attachedPage);

        AttachmentLocation AttachmentPosition { get; }
    }
}
