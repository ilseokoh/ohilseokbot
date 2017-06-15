using OhIlSeokBot.KakaoPlusFriend.Models;
using Microsoft.Bot.Connector.DirectLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OhIlSeokBot.KakaoPlusFriend.Helpers
{
    public static class MessageConvertor
    {
        public static Models.MessageResponse DirectLineToKakao(IList<Activity> activities)
        {
            if (activities == null || activities.Count <= 0) return null;

            var msg = new Models.MessageResponse();

            foreach (var activity in activities)
            {
                if (activity.Type != ActivityTypes.Message) continue;

                if (msg.message == null) msg.message = new Message();
                // 텍스트 메시지를 계속 누적 시킴
                msg.message.text += "\n" + activity.Text;

                if (activity.Attachments != null && activity.Attachments.Count > 0)
                {
                    foreach (Attachment attachment in activity.Attachments)
                    {
                        switch (attachment.ContentType)
                        {
                            case "application/vnd.microsoft.card.hero":
                                var heroCard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());
                                // hero 카드의 텍스트가 있다면 text 뒤에 붙여줌 
                                if (!string.IsNullOrEmpty(heroCard.Text.Trim()))
                                {
                                    msg.message.text += "\n\n" + heroCard.Text;
                                }
                                // 이미지가 여러개면 모두 표시를 못해줌. 
                                // 처음한개만 가져옴. width hegith 없는데 어떻게 표시될지? 
                                
                                if (heroCard.Images != null)
                                {
                                    var img = heroCard.Images.FirstOrDefault();
                                    if (msg.message == null) msg.message = new Message();
                                    msg.message.photo = new Photo
                                    {
                                        url = img.Url,
                                        width = 1000,
                                        height = 1000
                                    };
                                }
                                // 액션 버튼도 한개만 표시 가능함. OpenUrl 처음 한개만 취급하기로 함. 
                                // 
                                if (heroCard.Buttons != null)
                                {
                                    var herobutton = heroCard.Buttons.Where(x => x.Type == ActionTypes.OpenUrl).FirstOrDefault();
                                    if (herobutton != null)
                                    {
                                        if (msg.message == null) msg.message = new Message();

                                        msg.message.message_button = new MessageButton
                                        {
                                            label = herobutton.Title,
                                            url = herobutton.Value.ToString()
                                        };
                                    }

                                    var heroactionbutton = heroCard.Buttons.Where(x => x.Type == ActionTypes.ImBack).ToList();
                                    if (msg.keyboard == null)
                                    {
                                        msg.keyboard = new Keyboard
                                        {
                                            buttons = new string[] { },
                                            type = "buttons"
                                        };
                                        List<string> buttons = new List<string>();
                                        foreach (var actionbutton in heroactionbutton)
                                        {
                                            buttons.Add(actionbutton.Value.ToString());
                                        }
                                        msg.keyboard.buttons = buttons.ToArray();
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            return msg;
        }

        public static Models.MessageResponse DirectLineToKakao(Activity activity)
        {
            if (activity == null) return null;

            var msg = new Models.MessageResponse(); 

            if (activity.Type == ActivityTypes.Message)
            {
                if (msg.message == null) msg.message = new Message();
                // 텍스트 메시지 
                msg.message.text = activity.Text;

                // 이미지 
                if (activity.Attachments != null && activity.Attachments.Count > 0)
                {
                    foreach (Attachment attachment in activity.Attachments)
                    {
                        switch(attachment.ContentType)
                        {
                            case "image/png":
                            case "image/jpeg":
                                // activity는 attachment가 배열로 여러개가 오지만 Kakao는 한개만 가능.
                                // 따라서 처음 하나만 보여지는 걸로 ... 
                                if (msg.message.photo == null)
                                {
                                    // width, height를 주개 되어 있는데 줄 방법이 다운로드하고 열어서 헤더를 열어봐야 하지만 
                                    // http://stackoverflow.com/questions/30054517/get-image-dimensions-directly-from-an-url-c-sharp
                                    // 우선 설정하지 않고 보내면 카카오톡이 어찌 표시해주겠지 ... 
                                    msg.message.photo = new Photo
                                    {
                                        url = attachment.ContentUrl
                                    };
                                }
                                break;

                            case "application/vnd.microsoft.card.hero":
                                var heroCard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());
                                var img = heroCard.Images.FirstOrDefault();
                                if (img != null && msg.message.photo == null)
                                {
                                    msg.message.photo = new Photo
                                    {
                                        url = img.Url
                                    };
                                }
                                if(heroCard.Buttons != null)
                                {
                                    List<string> buttons = new List<string>();
                                    foreach(CardAction action in heroCard.Buttons)
                                    {
                                        if (action.Type == ActionTypes.OpenUrl)
                                        {
                                            if (msg.message.message_button == null)
                                            {
                                                msg.message.message_button = new MessageButton
                                                {
                                                    label = action.Title,
                                                    url = action.Value.ToString()
                                                };
                                            }
                                        }
                                        else if (action.Type == ActionTypes.PostBack)
                                        {
                                            buttons.Add(action.Value.ToString());
                                        }
                                    }

                                    if (msg.keyboard == null)
                                    {
                                        msg.keyboard = new Keyboard
                                        {
                                            buttons = new string[] { },
                                            type = "buttons"
                                        };
                                        msg.keyboard.buttons = buttons.ToArray();
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            return msg;
        }
    }
}