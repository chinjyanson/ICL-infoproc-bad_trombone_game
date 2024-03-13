import boto3

def delete_tables(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name="us-east-1")

    # Delete 'game' table
    game_table = dynamodb.Table('game')
    game_table.delete()
    print("Game table deleted.")

    # Delete 'Music' table
    music_table = dynamodb.Table('Music')
    music_table.delete()
    print("Music table deleted.")

if __name__ == '__main__':
    delete_tables()
