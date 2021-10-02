use serde_json;
use serde_json::Value;
use ureq;

use std::process::Command;

#[macro_use]
mod color;
use color::Color;

const VERSION: &str = "0.1.0";
const VERSION_JSON_URI: &str =
    "https://raw.githubusercontent.com/Basicprogrammer10/2B2T-Queue-Notifier/master/version.json";

fn main() {
    println!(
        "{} {}\n",
        color::color_bold("[*] Starting 2B2T-Queue-Notifier Installer", Color::Green),
        color::color_bold("V{}", Color::Cyan).replace("{}", VERSION)
    );

    color_print!(Color::Magenta, "[*] Getting Version File");
    let version = ureq::get(VERSION_JSON_URI)
        .call()
        .unwrap()
        .into_string()
        .unwrap();

    let version_json: Value = serde_json::de::from_str(&version).unwrap();
    let latest_version = version_json.get("latest").unwrap().as_str().unwrap();

    println!(
        "{} {}",
        color::color(" └─── Latest Version:", Color::Blue),
        color::color(latest_version, Color::Cyan)
    );

    // Check if .NET Core is installed
    color_print!(Color::Magenta, "[*] Checking if .NET Core is installed");
    let dotnet_core_installed = Command::new("dotnet")
        .arg("--list-runtimes")
        .output()
        .unwrap();
    // println!("{:?}", dotnet_core_installed);

    if dotnet_core_installed.status.success() {
        color_print!(Color::Magenta, "[*] .NET Core is installed");

        // Get .NET core Versions
        let versions = String::from_utf8_lossy(&dotnet_core_installed.stdout).replace('\r', "");
        let versions = versions
            .split('\n')
            .filter(|i| i.contains("Microsoft.NETCore.App"))
            .collect::<Vec<&str>>();
        let mut loop_count = 0;

        color_print!(Color::Blue, " └─── Found Versions");
        for i in versions.iter() {
            loop_count += 1;
            let version = i.split(' ').collect::<Vec<&str>>();
            if loop_count >= versions.len() - 1 {
                println!(
                    "{} {} {}",
                    color::color("     └───", Color::Blue),
                    color::color(version[1], Color::Cyan),
                    color::color("( LATEST )", Color::Green)
                );
                break;
            }
            println!(
                "{} {}",
                color::color("     ├───", Color::Blue),
                color::color(version[1], Color::Cyan)
            );
        }

        color_print!(Color::Magenta, "[*] Downloading x86");
        return;
    }
    color_print!(Color::Red, "[*] .NET Core is not installed");
}