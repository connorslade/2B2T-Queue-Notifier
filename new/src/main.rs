use std::env::consts;

use iced::{window, Application, Settings};

#[macro_use]
mod application;
mod misc;
mod panic;
mod queue;
mod settings;
mod style;
use application::Queue;
use misc::{assets, common};

pub const VERSION: &str = "α3.0.0";
pub const CFG_PATH: &str = ".2B2T-Queue-Notifier/config.cfg";

pub fn main() -> iced::Result {
    println!("[*] 2B2T Queue Notifier {}", VERSION);

    // Set Panic Handler
    panic::set_handler();

    // Load Window Icon
    let icon = image::load_from_memory(assets::ICON).unwrap();

    // Run Application
    Queue::run(Settings {
        window: window::Settings {
            size: (800, 400),
            min_size: Some((800, 400)),
            icon: Some(
                window::Icon::from_rgba(icon.to_rgba8().into_raw(), icon.width(), icon.height())
                    .unwrap(),
            ),
            ..Default::default()
        },
        default_font: Some(assets::MAIN_FONT_RAW),
        ..Settings::default()
    })
}
