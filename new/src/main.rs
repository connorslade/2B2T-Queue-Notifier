use iced::{
    button, window, Align, Button, Color, Column, Container, Element, Length, Sandbox, Settings,
    Text,
};

mod style;

const VERSION: &str = "α0.0.0";

pub fn main() -> iced::Result {
    Queue::run(Settings {
        window: window::Settings {
            size: (800, 400),
            ..Default::default()
        },
        ..Settings::default()
    })
}

#[derive(Default)]
struct Queue {
    position: u32,
    queue_color: Color,

    settings_button: button::State,
    debug_button: button::State,
}

#[derive(Debug, Clone, Copy)]
enum Message {
    OpenSettings,
    SetPosition(u32),
}

impl Sandbox for Queue {
    type Message = Message;

    fn new() -> Self {
        Self::default()
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {}", VERSION)
    }

    fn update(&mut self, message: Message) {
        match message {
            Message::OpenSettings => {
                println!("Open settings");
            }

            Message::SetPosition(position) => {
                self.position = position;
                self.queue_color = update_color(position);
            }
        }
    }

    fn view(&mut self) -> Element<Message> {
        let content = Column::new()
            .padding(20)
            .align_items(Align::Center)
            .push(
                Text::new(format!("{:0>3}", self.position))
                    .size(200)
                    .color(self.queue_color)
                    .font(iced::Font::External {
                        name: "JetBrainsMono-Regular.ttf",
                        bytes: include_bytes!("../assets/fonts/JetBrainsMono-Regular.ttf"),
                    }),
            )
            .push(
                Button::new(&mut self.settings_button, Text::new("Settings"))
                    .on_press(Message::OpenSettings),
            )
            .push(
                Button::new(&mut self.debug_button, Text::new("Debug"))
                    .on_press(Message::SetPosition(self.position + 100)),
            );

        Container::new(content)
            .width(Length::Fill)
            .height(Length::Fill)
            .center_x()
            .center_y()
            .style(style::Theme::Dark)
            .into()
    }
}

fn update_color(pos: u32) -> Color {
    if pos >= 500 {
        return Color::from_rgb8(191, 97, 106);
    }

    if pos >= 250 {
        return Color::from_rgb8(235, 203, 139);
    }

    Color::from_rgb8(163, 190, 140)
}
