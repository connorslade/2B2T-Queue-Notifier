use iced::{
    Align, Button, Checkbox, Column, Container, Length, Radio, Row, Rule, Scrollable, Slider, Text,
    TextInput,
};
use iced_native::widget::Widget;
use regex::Regex;

use super::{Message, Queue};
use crate::{
    misc::{assets, common},
    queue,
    settings::ConfigUpdate,
    style::{TextColor, Theme},
};

pub fn queue(this: &mut Queue) -> Container<Message> {
    Container::new(
        Column::new()
            .padding(20)
            .align_items(Align::Center)
            .push(
                Text::new(match this.position {
                    queue::Queue::Offline => "…".to_owned(),
                    queue::Queue::Online => "Online".to_owned(),
                    queue::Queue::Queue(i) => i.to_string(),
                })
                .size(200)
                .color(this.queue_color)
                .font(assets::QUEUE_FONT),
            )
            .push(
                Button::new(&mut this.settings_button, Text::new("Settings").size(25))
                    .style(this.config.theme)
                    .on_press(Message::OpenSettings),
            ),
    )
    .width(Length::Fill)
    .height(Length::Fill)
    .center_x()
    .center_y()
}

pub fn settings(this: &mut Queue) -> Container<Message> {
    Container::new(
        Column::new()
            .padding(20)
            .spacing(10)
            .push(
                Text::new("Settings")
                    .size(40)
                    .color(this.config.theme.text_color()),
            )
            .push(
                Scrollable::new(&mut this.config_scroller)
                    .spacing(10)
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Timeout")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::FillPortion(1)),
                            )
                            .push(
                                Slider::new(
                                    &mut this.timeout_slider,
                                    0.0..=100.0,
                                    this.config.timeout as f64,
                                    |x| Message::SettingsUpdate(ConfigUpdate::Timeout(x as u64)),
                                )
                                .width(Length::FillPortion(4))
                                .style(this.config.theme),
                            )
                            .push(Text::new(format!("[ {:0>3} ]", this.config.timeout))),
                    )
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Online Timeout")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::FillPortion(1)),
                            )
                            .push(
                                Slider::new(
                                    &mut this.online_timeout_slider,
                                    0.0..=100.0,
                                    this.config.online_timeout as f64,
                                    |x| {
                                        Message::SettingsUpdate(ConfigUpdate::OnlineTimeout(
                                            x as u64,
                                        ))
                                    },
                                )
                                .width(Length::FillPortion(4))
                                .style(this.config.theme),
                            )
                            .push(Text::new(format!("[ {:0>3} ]", this.config.online_timeout))),
                    )
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Log File")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::FillPortion(1)),
                            )
                            .push(
                                TextInput::new(
                                    &mut this.log_file_path_input,
                                    "",
                                    &this.config.log_file_path,
                                    |x| Message::SettingsUpdate(ConfigUpdate::LogFilePath(x)),
                                )
                                .width(Length::FillPortion(4))
                                .style(this.config.theme),
                            ),
                    )
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Chat Regex")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::FillPortion(1)),
                            )
                            .push(
                                TextInput::new(
                                    &mut this.chat_regex_input,
                                    "",
                                    this.config.chat_regex.as_str(),
                                    |x| match Regex::new(&x) {
                                        Ok(i) => {
                                            Message::SettingsUpdate(ConfigUpdate::ChatRegex(i))
                                        }
                                        Err(e) => {
                                            msgbox::create(
                                                "Regex Error",
                                                e.to_string().as_str(),
                                                msgbox::IconType::Info,
                                            )
                                            .unwrap();
                                            Message::None
                                        }
                                    },
                                )
                                .width(Length::FillPortion(4))
                                .style(this.config.theme),
                            ),
                    )
                    .push(Rule::horizontal(16).style(this.config.theme))
                    .push(
                        Row::new()
                            .spacing(10)
                            .push(Text::new("Toasts").size(25).width(Length::FillPortion(2)))
                            .push(
                                Checkbox::new(
                                    this.config.toast_settings.send_on_login,
                                    "Login",
                                    |x| Message::SettingsUpdate(ConfigUpdate::SendOnLogin(x)),
                                )
                                .width(Length::FillPortion(1))
                                .style(this.config.theme),
                            )
                            .push(
                                Checkbox::new(
                                    this.config.toast_settings.send_on_logout,
                                    "Logout",
                                    |x| Message::SettingsUpdate(ConfigUpdate::SendOnLogout(x)),
                                )
                                .width(Length::FillPortion(1))
                                .style(this.config.theme),
                            )
                            .push(
                                Checkbox::new(
                                    this.config.toast_settings.send_on_position_change,
                                    "Position Change",
                                    |x| {
                                        Message::SettingsUpdate(ConfigUpdate::SendOnPositionChange(
                                            x,
                                        ))
                                    },
                                )
                                .width(Length::FillPortion(1))
                                .style(this.config.theme),
                            ),
                    )
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Pos Change")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::FillPortion(1)),
                            )
                            .push(
                                Slider::new(
                                    &mut this.position_change_slider,
                                    0.0..=100.0,
                                    this.config.toast_settings.position_change_start as f64,
                                    |x| {
                                        Message::SettingsUpdate(ConfigUpdate::PositionChangeStart(
                                            x as u32,
                                        ))
                                    },
                                )
                                .width(Length::FillPortion(4))
                                .style(this.config.theme),
                            )
                            .push(Text::new(common::tir(
                                this.config.toast_settings.position_change_start > 0,
                                format!(
                                    "[ {:0>3} ]",
                                    this.config.toast_settings.position_change_start
                                )
                                .as_str(),
                                "[ ALL ]",
                            ))),
                    )
                    .push(Rule::horizontal(16).style(this.config.theme))
                    .push(
                        Row::new()
                            .spacing(20)
                            .push(
                                Text::new("Theme")
                                    .size(25)
                                    .color(this.config.theme.text_color())
                                    .width(Length::Fill),
                            )
                            .push(Radio::new(
                                Theme::Dark,
                                "Dark",
                                Some(this.config.theme),
                                Message::UpdateTheme,
                            ))
                            .push(Radio::new(
                                Theme::Light,
                                "Light",
                                Some(this.config.theme),
                                Message::UpdateTheme,
                            )),
                    ),
            )
            // .push(Space::new(Length::Fill, Length::Fill))
            .push(
                Row::new()
                    .spacing(10)
                    .push(
                        Button::new(&mut this.save_button, Text::new("Save").size(25))
                            .on_press(Message::ConfigSave)
                            .style(this.config.theme),
                    )
                    .push(
                        Button::new(&mut this.reset_button, Text::new("Reset").size(25))
                            .on_press(Message::ConfigReset)
                            .style(this.config.theme),
                    )
                    .push(
                        Button::new(&mut this.exit_button, Text::new("Cancel").size(25))
                            .on_press(Message::ConfigExit)
                            .style(this.config.theme),
                    ),
            ),
    )
    .width(Length::Fill)
    .height(Length::Fill)
    .center_x()
}
