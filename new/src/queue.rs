use regex::Regex;
use std::fs;

use chrono::Local;
use chrono::NaiveTime;
use lazy_static::lazy_static;

lazy_static! {
    static ref CHAT_REGEX: Regex =
        Regex::new("\\[..:..:..\\] \\[Client thread/INFO\\]: \\[CHAT\\]").unwrap();
}

pub fn check(path: &str, queue_regex: Regex) -> Option<u32> {
    let file = fs::read_to_string(&path).unwrap();
    let file_meta = fs::metadata(&path).unwrap();
    if let Ok(i) = file_meta.modified() {
        if i.elapsed().unwrap().as_secs() > 60 * 60 * 24 {
            return None;
        }
    }

    for line in file.lines().rev() {
        if !CHAT_REGEX.is_match(line) {
            continue;
        }

        if let Some(i) = queue_regex.captures(line) {
            if !check_time_validity(line) {
                return None;
            }

            let num = i.get(1).unwrap().as_str();
            let num = num
                .parse()
                .unwrap_or_else(|_| panic!("Invalid number for queue posision: `{}`", num));
            return Some(num);
        }
    }

    None
}

fn check_time_validity(line: &str) -> bool {
    let time = line.split(']').next().unwrap().split('[').nth(1).unwrap();
    let now = Local::now();
    let time = NaiveTime::parse_from_str(time, "%H:%M:%S").unwrap();
    let diff = now.time().signed_duration_since(time);
    diff.num_seconds() >= 0 && diff.num_seconds() <= 10
}
