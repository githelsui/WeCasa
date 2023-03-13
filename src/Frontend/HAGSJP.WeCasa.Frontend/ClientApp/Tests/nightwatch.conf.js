module.exports = {
    src_folders: ["tests"],
    output_folder: "reports",
    custom_commands_path: "",
    custom_assertions_path: "",
    page_objects_path: "",
    globals_path: "",
  
    webdriver: {
      start_process: true,
      server_path: "node_modules/.bin/chromedriver",
      port: 9515,
    },
  
    test_settings: {
      default: {
        launch_url: "http://localhost",
        selenium_port: 44411,
        selenium_host: "localhost",
        desiredCapabilities: {
          browserName: "chrome",
        },
        screenshots: {
          enabled: false,
          path: "",
          on_failure: true,
          on_error: true,
        },
      },
    },
  };
  