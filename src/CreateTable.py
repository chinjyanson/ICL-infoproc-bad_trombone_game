import boto3

def create_game_table(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.create_table(
        TableName='game',
        KeySchema=[
            {
                'AttributeName': 'song',
                'KeyType': 'HASH'  # Partition key
            },
            {
                'AttributeName': 'username',
                'KeyType': 'RANGE'  # Partition key
            },
        ],
        AttributeDefinitions=[
            {
                'AttributeName': 'song',
                'AttributeType': 'S'
            },
            {
                'AttributeName': 'username',
                'AttributeType': 'S'
            },
        ],
        ProvisionedThroughput={
            'ReadCapacityUnits': 10,
            'WriteCapacityUnits': 10
        }
    )
    return table

def create_music_table(dynamodb=None):
    if not dynamodb:
        dynamodb = boto3.resource('dynamodb', region_name='us-east-1')

    table = dynamodb.create_table(
        TableName='Music',
        KeySchema=[
            {
                'AttributeName': 'name',
                'KeyType': 'HASH'  # Partition key
            },
            {
                'AttributeName': 'author',
                'KeyType': 'RANGE'  # Sort key
            }
        ],
        AttributeDefinitions=[
            {
                'AttributeName': 'name',
                'AttributeType': 'S'
            },
            {
                'AttributeName': 'author',
                'AttributeType': 'S'
            }
        ],
        ProvisionedThroughput={
            'ReadCapacityUnits': 100,
            'WriteCapacityUnits': 100
        }
    )
    return table

if __name__ == '__main__':
    game_table = create_game_table()
    print("Game Table status:", game_table.table_status)
    music_table = create_music_table()
    print("Music Table status:", music_table.table_status)

