use iced::Color;
#[cfg(not(windows))]
use notify_rust::Notification;
#[cfg(windows)]
use winrt_notification::{Duration, Sound, Toast};

use crate::queue::Queue;

pub fn update_color(pos: Queue) -> Color {
    match pos {
        Queue::Offline => Color::from_rgb8(191, 97, 106),
        Queue::Online => Color::from_rgb8(163, 190, 140),
        Queue::Queue(i) => {
            if i >= 500 {
                return Color::from_rgb8(191, 97, 106);
            }

            if i >= 250 {
                return Color::from_rgb8(235, 203, 139);
            }

            Color::from_rgb8(163, 190, 140)
        }
    }
}

pub fn send_basic_toast(test: &str) {
    #[cfg(windows)]
    Toast::new(Toast::POWERSHELL_APP_ID)
        .title(test)
        .sound(Some(Sound::Default))
        .duration(Duration::Short)
        .show()
        .expect("couldn't toast toast");

    #[cfg(not(windows))]
    Notification::new().summary(test).show().unwrap();
}
