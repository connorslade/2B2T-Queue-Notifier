use regex::Regex;
use std::fs;

use chrono::Local;
use chrono::NaiveTime;

pub fn check(path: String, queue_regex: String) -> Option<u32> {
    let file = fs::read_to_string(path).unwrap();
    let chat_regex = Regex::new("\\[..:..:..\\] \\[Client thread/INFO\\]: \\[CHAT\\]").unwrap();
    let queue_regex = Regex::new(&queue_regex).unwrap();
    let mut new_pos = None;
    for line in file.lines() {
        if !chat_regex.is_match(line) {
            continue;
        }

        if queue_regex.is_match(line) {
            let valid = check_time_validity(line);
            if !valid {
                continue;
            }

            new_pos = queue_regex
                .split(line)
                .nth(1)
                .unwrap_or_default()
                .trim()
                .parse()
                .ok();
        }
    }

    new_pos
}

fn check_time_validity(line: &str) -> bool {
    let time = line.split(']').next().unwrap().split('[').nth(1).unwrap();
    let now = Local::now();
    let time = NaiveTime::parse_from_str(time, "%H:%M:%S").unwrap();
    let diff = now.time().signed_duration_since(time);
    diff.num_seconds() <= 10
}
