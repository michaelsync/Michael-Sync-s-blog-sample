using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace ChatUdpUnicast {
    public class MockMainViewModel : MainPageViewModel {
        public MockMainViewModel() {
            Messages = new ObservableCollection<Message> {
                new Message() { Text = "Dude! Are you allright?'", Type = MessageType.FromOthers, UserName = "Leo", Timestamp = DateTime.Now },
                new Message() {  Text = "John joined the chat.", Type = MessageType.Notification, UserName = "Me", Timestamp = DateTime.Now },
                new Message() {  Text = "He was so drunk!", Type = MessageType.FromOthers, UserName = "John", Timestamp = DateTime.Now },
                new Message() { Text = "I wasn't that drunk last night!", Type = MessageType.Self, UserName = "Me", Timestamp = DateTime.Now },
                new Message() {  Text = "Dude, you were hugging an old man with a breard screeming 'DUMBLEDORE YOU'RE ALIVE'", 
                    Type = MessageType.FromOthers, UserName = "Leo", Timestamp = DateTime.Now },                
                 new Message() {  Text = "haha.", Type = MessageType.Self, UserName = "Me", Timestamp = DateTime.Now }
            };

        }

    }
}
