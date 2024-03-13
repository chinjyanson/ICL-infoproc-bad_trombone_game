import json
import boto3

def load_music(music_info, dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')
    table = dynamodb.Table('Music')
    
    try:
        # 将notes中的每个数值类型转换为字符串
        notes = [{
            'notePosition': str(note['notePosition']),
            'noteLength': str(note['noteLength']),
            'startPitch': str(note['startPitch']),
            'pitchDelta': str(note['pitchDelta']),
            'pitchEnd': str(note['pitchEnd']),
        } for note in music_info['notes']]
        
        # 准备要插入的项目数据
        item = {
            'name': music_info['name'],
            'author': music_info['author'],
            'year': str(music_info['year']),
            'genre': music_info['genre'],
            'description': music_info['description'],
            'difficulty': str(music_info['difficulty']),
            'tempo': str(music_info['tempo']),
            'notes': notes,
        }

        print(f"Adding music: {item['name']}, Artist: {item['author']}")
        table.put_item(Item=item)
        print("Music added successfully.")
        
        # 打印已插入 DynamoDB 表的音符信息
        print("Inserted notes:")
        for note in notes:
            print(note)
        
    except Exception as e:
        print(f"Error occurred while loading music: {e}")

if __name__ == '__main__':
    try:
        with open("twinkle.json") as json_file:
            music_info = json.load(json_file, parse_float=str)  # Here ensure that floating numbers are parsed as strings
            print("JSON file loaded successfully.")
            load_music(music_info)
        with open("5Sym.json") as json_file:
            music_info = json.load(json_file, parse_float=str)  # Here ensure that floating numbers are parsed as strings
            print("JSON file loaded successfully.")
            load_music(music_info)
        with open("MissionIm.json") as json_file:
            music_info = json.load(json_file, parse_float=str)  # Here ensure that floating numbers are parsed as strings
            print("JSON file loaded successfully.")
            load_music(music_info)
    except FileNotFoundError:
        print("Error: JSON file not found.")
    except json.JSONDecodeError:
        print("Error: JSON file could not be decoded.")
    except Exception as e:
        print(f"Unexpected error occurred: {e}")