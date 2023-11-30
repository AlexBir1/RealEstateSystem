export interface ApartmentPhotoModel{
    id: string;

    imageUrl: string;
    photoFile: Blob;
    
    apartmentId: string;
    
    creationDate: Date;
    lastlyUpdatedDate: Date;
}