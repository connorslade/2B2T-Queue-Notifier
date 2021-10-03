use std::env::consts;
use std::panic;
use std::process;

use iced::{
    button, window, Align, Button, Color, Column, Container, Element, Length, Sandbox, Settings,
    Text,
};
use msgbox;

#[macro_use]
mod common;
mod settings;
mod style;

pub const VERSION: &str = "α0.0.0";

pub fn main() -> iced::Result {
    panic::set_hook(Box::new(|p| {
        msgbox::create(
            "2B2T-Queue-Notifier Error",
            &format!(
                "{}\nCompile Time: {}\nPlatform: {} {}\nVersion: {}",
                p.to_string(),
                include_str!("../build_data/compile_time"),
                consts::OS,
                consts::ARCH,
                VERSION,
            ),
            msgbox::IconType::Error,
        )
        .unwrap_or_default();
        process::exit(-1);
    }));

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
    position: Option<u32>,
    queue_color: Color,

    settings_button: button::State,
    debug_button: button::State,
}

#[derive(Debug, Clone, Copy)]
enum Message {
    OpenSettings,
    SetPosition(u32),
}

enum View {
    Queue,
    Settings,
    Debug,
}

impl Sandbox for Queue {
    type Message = Message;

    fn new() -> Self {
        Self {
            position: None,
            queue_color: Color::from_rgb8(191, 97, 106),
            ..Default::default()
        }
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {}", VERSION)
    }

    fn update(&mut self, message: Message) {
        match message {
            Message::OpenSettings => {
                // Open a settings window
                panic!("Settings not implemented yet!");
            }

            Message::SetPosition(position) => {
                if self.position == Some(position) {
                    return;
                }

                self.position = Some(position);
                self.queue_color = update_color(position);
            }
        }
    }

    fn view(&mut self) -> Element<Message> {
        let font = iced::Font::External {
            name: "JetBrainsMono-Medium.ttf",
            bytes: include_bytes!("../assets/fonts/JetBrainsMono-Medium.ttf"),
        };

        let content = Column::new()
            .padding(20)
            .align_items(Align::Center)
            .push(
                Text::new(match self.position {
                    Some(position) => position.to_string(),
                    None => "…".to_string(),
                })
                .size(200)
                .color(self.queue_color)
                .font(font),
            )
            .push(
                Button::new(
                    &mut self.settings_button,
                    Text::new("Settings").size(25).font(font),
                )
                .on_press(Message::OpenSettings),
            )
            .push(
                Button::new(
                    &mut self.debug_button,
                    Text::new("Debug").size(25).font(font),
                )
                .on_press(Message::SetPosition(self.position.unwrap_or(0) + 25)),
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
