use std::env::consts;
use std::panic;
use std::process;

use iced::{window, Application, Settings};
use image::GenericImageView;

#[macro_use]
mod common;
mod application;
mod assets;
mod settings;
mod style;
use application::Queue;

pub const VERSION: &str = "α3.0.0";
pub const CFG_PATH: &str = ".2B2T-Queue-Notifier/config.cfg";

pub fn main() -> iced::Result {
    println!("[*] 2B2T Queue Notifier {}", VERSION);

    // Set Panic Handler
    panic::set_hook(Box::new(|p| {
        eprintln!("{}", p);
        msgbox::create(
            "2B2T-Queue-Notifier Error",
            &format!(
                "{}\n{}\nCompile Time: {}\nPlatform: {} {}\nVersion: {}",
                p.to_string(),
                env!("GIT_INFO"),
                env!("COMPILE_TIME"),
                consts::OS,
                consts::ARCH,
                VERSION,
            ),
            msgbox::IconType::Error,
        )
        .unwrap_or_default();
        process::exit(-1);
    }));

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
