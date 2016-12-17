using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace FormFlow
{
    [Serializable]
    public class ChoiceDialog : IDialog<string>
    {
        private readonly string msg;
        private readonly List<string> opts = new List<string>();
        public ChoiceDialog(string msg, IEnumerable<string> opts)
        {
            this.msg = msg;
            foreach (var x in opts) { this.opts.Add(x); }
        }
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync(msg);
            var O = new PromptOptions<string>(msg, options: opts);
            PromptDialog.Choice(context, ProcessResult, O);
        }

        private async Task ProcessResult(IDialogContext context, IAwaitable<object> result)
        {
            var res = await result;
            context.Done((string)res);
        }
    }
}