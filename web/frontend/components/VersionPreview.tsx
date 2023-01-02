import { Flex, Heading, Icon, Text } from '@chakra-ui/react'
import { FC } from 'react'
import TimeAgo from 'javascript-time-ago'
import en from 'javascript-time-ago/locale/en'
import { GoFileCode } from 'react-icons/go'

const timeAgo = new TimeAgo('en-US')

interface Props {
  packageName: string
  version: string
  uploadedTime: number
  isSmall?: boolean
}

const VersionPreview: FC<Props> = ({ packageName, version, uploadedTime, isSmall = false }) => {
  const imgSize = isSmall ? '42px' : '56px'
  const deltaT = timeAgo.format(new Date(uploadedTime), 'twitter')
  return (
    <a href={`/api/v1/packages/name/${packageName}/versions/${version}/raw`}>
      <Flex mt='10px' mr={{ sm: '0px', xl: '100px' }} border='2px solid #6e97b4' p='15px' w='100%' borderRadius='5px'>
        <Icon as={GoFileCode} fontSize={imgSize} color='lightBlueHero' />
        <Flex flexDir='column' justifyContent='center' ml='20px'>
          <Heading as='h6' fontWeight='normal' fontSize={isSmall ? '20px' : '24px'}>
            v{version}
          </Heading>
          <Text fontSize='14px' color='gray'>
            Uploaded {deltaT} ago
          </Text>
        </Flex>
      </Flex>
    </a>
  )
}

export default VersionPreview
