use std::fmt::Display;

use iced::button;
use iced::checkbox;
use iced::container;
use iced::slider;
use iced::text_input;

mod dark;

pub trait TextColor {
    fn text_color(&self) -> iced::Color;
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum Theme {
    Light,
    Dark,
}

impl Theme {
    pub const ALL: [Theme; 2] = [Theme::Light, Theme::Dark];

    pub fn from_string(str: String) -> Option<Theme> {
        match str.to_lowercase().as_str() {
            "light" => Some(Theme::Light),
            "dark" => Some(Theme::Dark),
            _ => None,
        }
    }
}

impl Display for Theme {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match *self {
            Theme::Dark => f.write_str("Dark"),
            Theme::Light => f.write_str("Light"),
        }
    }
}

impl Default for Theme {
    fn default() -> Theme {
        Theme::Dark
    }
}

impl TextColor for Theme {
    fn text_color(&self) -> iced::Color {
        match self {
            Theme::Light => iced::Color::from_rgb8(0, 0, 0),
            Theme::Dark => iced::Color::from_rgb8(255, 255, 255),
        }
    }
}

impl From<Theme> for Box<dyn container::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::Container.into(),
            Theme::Light => Default::default(),
        }
    }
}

impl From<Theme> for Box<dyn text_input::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::TextInput.into(),
            Theme::Light => Default::default(),
        }
    }
}

impl From<Theme> for Box<dyn button::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::Button.into(),
            Theme::Light => Default::default(),
        }
    }
}

impl From<Theme> for Box<dyn slider::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::Slider.into(),
            Theme::Light => Default::default(),
        }
    }
}

impl From<Theme> for Box<dyn checkbox::StyleSheet> {
    fn from(theme: Theme) -> Self {
        match theme {
            Theme::Dark => dark::Checkbox.into(),
            Theme::Light => Default::default(),
        }
    }
}
