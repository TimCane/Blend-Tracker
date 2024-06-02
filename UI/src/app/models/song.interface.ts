export interface Song {
    id: string
    title: string
    artists: Artist[]
    album: Album
    addedBy: AddedBy[]
    explicit: boolean
    duration: number
    firstSeen: string
    lastSeen: string
}

interface Artist {
    id: string
    name: string
}

interface Album {
    id: string
    name: string
    image: string
}

interface AddedBy {
    id: string
    addedOn: string
}
