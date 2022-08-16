use std::path::Path;

use home::home_dir;
use iced::{
    button, executor, scrollable, slider, text_input, time, Application, Clipboard, Color, Command,
    Element,
};

use super::{
    common, queue,
    settings::{Config, ConfigUpdate},
    style::Theme,
    CFG_PATH, VERSION,
};

mod ui;

#[derive(Default)]
pub struct Queue {
    position: queue::Queue,
    queue_color: Color,
    config: Config,
    view: View,

    // Ui elements
    settings_button: button::State,

    // Config Stuff
    old_config: Option<Config>,
    config_scroller: scrollable::State,
    save_button: button::State,
    exit_button: button::State,
    reset_button: button::State,

    timeout_slider: slider::State,
    online_timeout_slider: slider::State,
    position_change_slider: slider::State,
    log_file_path_input: text_input::State,
    chat_regex_input: text_input::State,
}

#[derive(Debug, Clone)]
pub enum Message {
    SettingsUpdate(ConfigUpdate),
    UpdateTheme(Theme),
    OpenSettings,
    ConfigReset,
    ConfigSave,
    ConfigExit,
    Tick,

    None,
}

enum View {
    Queue,
    Settings,
}

impl Application for Queue {
    type Executor = executor::Default;
    type Message = Message;
    type Flags = ();

    fn new(_flags: ()) -> (Queue, Command<Message>) {
        let config_path = home_dir().unwrap().join(Path::new(CFG_PATH));

        let config = match Config::load(config_path.clone()) {
            Some(config) => {
                println!("[*] Successfully Read Config");
                config
            }
            None => {
                match config_path.exists() {
                    false => println!("[*] Config File Not Found. Using Defaults"),
                    true => println!("[*] Error Parsing Config File. Using Defaults"),
                }
                Config::default()
            }
        };

        (
            Self {
                position: queue::Queue::Offline,
                queue_color: Color::from_rgb8(191, 97, 106),
                config,
                ..Default::default()
            },
            Command::none(),
        )
    }

    fn title(&self) -> String {
        format!("2B2T-Queue-Notifier {}", VERSION)
    }

    fn update(&mut self, message: Message, _clipboard: &mut Clipboard) -> Command<Message> {
        match message {
            Message::OpenSettings => {
                self.view = View::Settings;
                self.old_config = Some(self.config.clone());
            }

            Message::UpdateTheme(theme) => {
                self.config.theme = theme;

                // :P
                if theme == Theme::Light {
                    msgbox::create(
                        "you have a problem",
                        "light mode... really?",
                        msgbox::IconType::Info,
                    )
                    .unwrap();
                }
            }

            Message::SettingsUpdate(config_update) => {
                self.config = self.config.apply_update(config_update);
            }

            Message::ConfigSave => {
                self.config
                    .save(home_dir().unwrap().join(Path::new(CFG_PATH)));
                self.view = View::Queue;
            }

            Message::ConfigExit => {
                self.view = View::Queue;
                if let Some(i) = self.old_config.take() {
                    self.config = i;
                }
            }

            Message::ConfigReset => {
                self.config = Config::default();
            }

            Message::Tick => {
                let new_pos = queue::check(&self.config);

                if self.position == new_pos {
                    return Command::none();
                }

                self.queue_color = common::update_color(new_pos);
                self.position = new_pos;

                match new_pos {
                    queue::Queue::Offline => {
                        println!("[E] Disconnected");
                        if self.config.toast_settings.send_on_logout {
                            common::send_basic_toast("❌ Disconnected\n＞︿＜")
                        }
                    }
                    queue::Queue::Online => {
                        println!("[E] Logged In!");
                        if self.config.toast_settings.send_on_login {
                            common::send_basic_toast("✅ You have logged in!")
                        }
                    }
                    queue::Queue::Queue(i) => {
                        println!("[E] In Queue Pos {}", i);
                        if self.config.toast_settings.send_on_position_change {
                            common::send_basic_toast(&format!("⏰ Queue: {}", i));
                        }
                    }
                }
            }

            Message::None => {}
        };
        Command::none()
    }

    fn subscription(&self) -> iced::Subscription<Self::Message> {
        time::every(std::time::Duration::from_millis(500)).map(|_| Message::Tick)
    }

    fn view(&mut self) -> Element<Message> {
        let theme = self.config.theme;
        match self.view {
            View::Queue => ui::queue(self),
            View::Settings => ui::settings(self),
        }
        .style(theme)
        .into()
    }
}

impl Default for View {
    fn default() -> Self {
        View::Queue
    }
}
