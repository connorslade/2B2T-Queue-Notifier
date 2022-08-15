use regex::Regex;
use std::fs;

use chrono::Local;
use chrono::NaiveTime;
use lazy_static::lazy_static;

use crate::settings::Config;

lazy_static! {
    static ref CHAT_REGEX: Regex =
        Regex::new("\\[..:..:..\\] \\[Client thread/INFO\\]: \\[CHAT\\]").unwrap();
}

#[derive(PartialEq, Eq, Debug, Default, Clone, Copy)]
pub enum Queue {
    #[default]
    Offline,
    Online,
    Queue(u32),
}

pub fn check(config: &Config) -> Queue {
    let raw_file = fs::read(&config.log_file_path).unwrap();
    let file = String::from_utf8_lossy(&raw_file);
    let file_meta = fs::metadata(&config.log_file_path).unwrap();
    if let Ok(i) = file_meta.modified() {
        if i.elapsed().unwrap().as_secs() > 60 * 60 * 24 {
            return Queue::Offline;
        }
    }

    for line in file.lines().rev() {
        if !CHAT_REGEX.is_match(line) {
            continue;
        }

        let queue = config.chat_regex.captures(line);
        let timeframe = match queue {
            Some(_) => config.timeout,
            None => config.online_timeout
        };
        let time = match get_time_validity(line){
            Some(i) => i,
            None => return Queue::Offline
        };

        if time > timeframe {
            return Queue::Offline;
        }
        
        if let Some(i) = queue {
            let num = i.get(1).unwrap().as_str();
            let num = num
                .parse()
                .unwrap_or_else(|_| panic!("Invalid number for queue posision: `{}`", num));
            return Queue::Queue(num);
        }

        return Queue::Online;
    }

    Queue::Offline
}

fn get_time_validity(line: &str) -> Option<u64> {
    let time = line.split(']').next().unwrap().split('[').nth(1).unwrap();
    let now = Local::now();
    let time = NaiveTime::parse_from_str(time, "%H:%M:%S").unwrap();
    let diff = now.time().signed_duration_since(time);
    
    if diff.num_seconds() >= 0 {
        return Some(diff.num_seconds() as u64);
    }

    None
}
