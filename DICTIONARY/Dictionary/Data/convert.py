import csv
import json

def read_csv(filename):
    with open(filename, "r") as f:
        reader = csv.reader(f)
        data = []
        for row in reader:
            data.append(row)
        return data

def convert_csv_to_json(data):
    json_data = []
    for row in data:
        json_obj = {}
        json_obj["vocab"] = row[0]
        json_obj["meaning"] = row[1]
        json_obj["example"] = row[2]
        json_data.append(json_obj)
    return json_data

def write_json(data, filename):
    with open(filename, "w") as f:
        json.dump(data, f, indent=4)

def main():
    input_filename = "it_topic.csv"
    output_filename = "it_topic.json"

    csv_data = read_csv(input_filename)
    json_data = convert_csv_to_json(csv_data)
    write_json(json_data, output_filename)

if __name__ == "__main__":
    main()
