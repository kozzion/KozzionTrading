
// dimensions layout: 
// 0-2 target offset_size        (flat)
// 3-5 source and inclusion size (incremental)
// 6-8 filter size               (not incremental)
// 9   sample_count
__global__ void cuda_filter_tips_fast(
	float *target_image,  
	float const *const source_image,
    float const *const inclusion_image, 	
    float const *const filter_kernel,      
    int const *const dimensions,	
    int const *const filter_source_offsets,  
    float const *const error_measure,
	unsigned int const *const real_grid_dimensions)
{
    //indexing
	unsigned int block_index_x           = blockIdx.x % real_grid_dimensions[0]; 
	unsigned int block_index_y           = blockIdx.x / real_grid_dimensions[0]; 
    unsigned int block_index_z           = blockIdx.y;
	unsigned int target_index_x          = threadIdx.x + (block_index_x * blockDim.x); 
    unsigned int target_index_y          = threadIdx.y + (block_index_y * blockDim.y);
	unsigned int target_index_z          = threadIdx.z + (block_index_z * blockDim.z);

	unsigned int target_index_frame_offset =   target_index_x + dimensions[0]+
		                                     ((target_index_y + dimensions[1]) * dimensions[3])+
								             ((target_index_z + dimensions[2]) * dimensions[4]);

    float total_contribution              = 0;
    for (int filter_index = 0; filter_index < dimensions[6]; filter_index++)
    {
        int source_index_frame_offset = target_index_frame_offset + filter_source_offsets[filter_index];
         if (inclusion_image[source_index_frame_offset] == 0.0f) 
        {
            continue;
        }
        // compute error, this is were the magic happens: 
        // choose any function that has range {0, 1} to determine contribution
        float mse = 0;
        for (int time_index = 0; time_index < dimensions[9]; time_index++)
        {
            float time_error = source_image[target_index_frame_offset + (time_index * dimensions[5])] - 
                               source_image[source_index_frame_offset + (time_index * dimensions[5])];
            mse += time_error * time_error;
        }
        // note magic number
        float k = (mse * mse)  / (43245000.0f);
        if (k >= 4.61f) 
        {
            continue;
        }

        float contribution = filter_kernel[filter_index] * inclusion_image[source_index_frame_offset] * expf(-k);

        // here the magic ends
        total_contribution += contribution;
        for (int time_index = 0; time_index < dimensions[9]; time_index++)
        {
            target_image[target_index_frame_offset + (time_index * dimensions[5])] += source_image[source_index_frame_offset + (time_index * dimensions[5])] * contribution;
        }
     
    }
    for (int time_index = 0; time_index < dimensions[9]; time_index++)
    {
        target_image[target_index_frame_offset + (time_index * dimensions[5])] /= total_contribution;

    }
}